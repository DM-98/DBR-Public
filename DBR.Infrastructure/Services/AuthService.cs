using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using DBR.Core.Enums;
using DBR.Core.Helpers;
using DBR.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DBR.Infrastructure.Services;

public class AuthService : IAuthService
{
	private readonly UserManager<Member> userManager;
	private readonly RoleManager<IdentityRole<Guid>> roleManager;
	private readonly IConfiguration configuration;

	public AuthService(UserManager<Member> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
	{
		this.userManager = userManager;
		this.roleManager = roleManager;
		this.configuration = configuration;
	}

	public async Task<ResponseDTO<Member>> RegisterAsync(RegisterInputModel registerInputModel)
	{
		Member? member = await userManager.FindByEmailAsync(registerInputModel.Email);

		if (member is not null)
		{
			return new ResponseDTO<Member> { Success = false, ErrorMessage = $"Bruger med e-mail: {registerInputModel.Email} eksisterer allerede.", ErrorType = ErrorType.EntityDuplicate };
		}

		Member memberToCreate = new()
		{
			Email = registerInputModel.Email,
			WorkshopId = registerInputModel.WorkshopId,
			UserName = registerInputModel.Name,
			IsTermsAccepted = registerInputModel.IsTermsAccepted,
			LockoutEnabled = true,
			SecurityStamp = Guid.NewGuid().ToString()
		};

		IdentityResult createUserResult = await userManager.CreateAsync(memberToCreate, registerInputModel.Password);

		if (!createUserResult.Succeeded)
		{
			return new ResponseDTO<Member> { Success = false, ErrorMessage = createUserResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
		}

		bool memberRole = await roleManager.RoleExistsAsync("Member");

		if (!memberRole)
		{
			await roleManager.CreateAsync(new IdentityRole<Guid>("Member"));
		}

		await userManager.AddToRoleAsync(memberToCreate, "Member");

		bool adminRole = await roleManager.RoleExistsAsync("Admin");

		if (!adminRole)
		{
			await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
		}

		if (memberToCreate.Email.ToLowerInvariant() is "admin@admin.com")
		{
			await userManager.AddToRoleAsync(memberToCreate, "Admin");
		}

		return new ResponseDTO<Member> { Success = true, Content = memberToCreate };
	}

	public async Task<ResponseDTO<Tuple<string, string>>> LoginAsync(LoginInputModel loginInputModel)
	{
		Member member = await userManager.FindByEmailAsync(loginInputModel.Email);

		if (member is null)
		{
			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = $"Bruger med e-mail ({loginInputModel.Email}) eksisterer ikke, prøv igen eller anmod om at blive registreret.", ErrorType = ErrorType.EntityNotFound };
		}

		bool isMemberLockedOut = await userManager.IsLockedOutAsync(member);

		if (isMemberLockedOut)
		{
			DateTimeOffset? dateTimeLockoutEnd = await userManager.GetLockoutEndDateAsync(member);
			DateTimeOffset dateTimeNow = DateTimeOffset.Now;
			TimeSpan timeSpanLockoutEnd = (TimeSpan)(dateTimeLockoutEnd - dateTimeNow);
			int minutesLockoutEnd = (int)Math.Ceiling((double)timeSpanLockoutEnd.TotalMinutes);
			int secondsLockoutEnd = (int)Math.Ceiling((double)timeSpanLockoutEnd.TotalSeconds);

			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = $"Du har forsøgt at logge ind for mange gange, prøv venligst igen om {(minutesLockoutEnd <= 1 ? secondsLockoutEnd : minutesLockoutEnd)} {(minutesLockoutEnd <= 1 ? "sekunder" : "minutter")}.", ErrorType = ErrorType.TooManyLoginAttempts };
		}

		bool isPasswordCorrect = await userManager.CheckPasswordAsync(member, loginInputModel.Password);

		if (!isPasswordCorrect)
		{
			await userManager.AccessFailedAsync(member);

			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = "Forkert adgangskode. Prøv igen.", ErrorType = ErrorType.InvalidInput };
		}

		List<Claim> userClaims = new()
		{
			new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
			new Claim(ClaimTypes.Name, member.UserName),
			new Claim(ClaimTypes.Email, member.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		List<string> userRoles = (List<string>)await userManager.GetRolesAsync(member);
		userRoles.ForEach(userRole => userClaims.Add(new Claim(ClaimTypes.Role, userRole)));

		JwtSecurityToken accessToken = AuthHelper.GenerateAccessToken(userClaims, configuration);
		string refreshToken = AuthHelper.GenerateRefreshToken();

		member.RefreshToken = refreshToken;
		member.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT:RefreshTokenExpiryInDays"]));

		IdentityResult updateResult = await userManager.UpdateAsync(member);

		await userManager.ResetAccessFailedCountAsync(member);

		return updateResult.Succeeded
			? new ResponseDTO<Tuple<string, string>> { Success = true, Content = Tuple.Create(new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken) }
			: new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = updateResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<Tuple<string, string>>> RefreshTokenAsync(Tuple<string, string> accessRefreshTokensToRefresh)
	{
		if (string.IsNullOrWhiteSpace(accessRefreshTokensToRefresh.Item1) || string.IsNullOrWhiteSpace(accessRefreshTokensToRefresh.Item2))
		{
			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = "Din session er udløbet. Du logges ud.", ErrorType = ErrorType.TokenNotFound };
		}

		ClaimsPrincipal? claimsPrincipal = AuthHelper.TryGetClaimsPrincipal(accessRefreshTokensToRefresh.Item1, configuration);

		if (claimsPrincipal is null)
		{
			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = "Din session er udløbet. Du logges ud.", ErrorType = ErrorType.ClaimsPrincipalNotFoundFromAccessToken };
		}

		Member memberToRefresh = await userManager.FindByIdAsync(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));

		if (memberToRefresh is null)
		{
			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = "Din session er udløbet. Du logges ud.", ErrorType = ErrorType.UserNotFoundWithClaimsPrincipal };
		}

		if (string.IsNullOrWhiteSpace(memberToRefresh.RefreshToken) || memberToRefresh.RefreshTokenExpiryTime <= DateTime.UtcNow)
		{
			return new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = "Din session er udløbet. Du logges ud.", ErrorType = ErrorType.RefreshTokenInvalidOrExpired };
		}

		JwtSecurityToken newAccessToken = AuthHelper.GenerateAccessToken(claimsPrincipal.Claims, configuration);
		string newRefreshToken = AuthHelper.GenerateRefreshToken();

		memberToRefresh.RefreshToken = newRefreshToken;
		memberToRefresh.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT:RefreshTokenExpiryInDays"]));

		IdentityResult updateResult = await userManager.UpdateAsync(memberToRefresh);

		return updateResult.Succeeded
			? new ResponseDTO<Tuple<string, string>> { Success = true, Content = Tuple.Create(new JwtSecurityTokenHandler().WriteToken(newAccessToken), newRefreshToken) }
			: new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = updateResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<Tuple<string, string>>> UpdateMemberAsync(Member updatedMember)
	{
		List<Claim> newUserClaims = new()
		{
			new Claim(ClaimTypes.NameIdentifier, updatedMember.Id.ToString()),
			new Claim(ClaimTypes.Name, updatedMember.UserName),
			new Claim(ClaimTypes.Email, updatedMember.Email),
			new Claim(ClaimTypes.MobilePhone, updatedMember.PhoneNumber),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		List<string> newUserRoles = (List<string>)await userManager.GetRolesAsync(updatedMember);
		newUserRoles.ForEach(userRole => newUserClaims.Add(new Claim(ClaimTypes.Role, userRole)));

		JwtSecurityToken newAccessToken = AuthHelper.GenerateAccessToken(newUserClaims, configuration);
		string newRefreshToken = AuthHelper.GenerateRefreshToken();

		updatedMember.RefreshToken = newRefreshToken;
		updatedMember.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JWT:RefreshTokenExpiryInDays"]));

		IdentityResult updateResult = await userManager.UpdateAsync(updatedMember);

		return updateResult.Succeeded
			? new ResponseDTO<Tuple<string, string>> { Success = true, Content = Tuple.Create(new JwtSecurityTokenHandler().WriteToken(newAccessToken), newRefreshToken) }
			: new ResponseDTO<Tuple<string, string>> { Success = false, ErrorMessage = updateResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<IdentityRole<Guid>>> CreateRoleAsync(string role)
	{
		IdentityRole<Guid> roleToCreate = new(role);
		IdentityResult createResult = await roleManager.CreateAsync(roleToCreate);

		return createResult.Succeeded
			? new ResponseDTO<IdentityRole<Guid>> { Success = true, Content = roleToCreate }
			: new ResponseDTO<IdentityRole<Guid>> { Success = false, ErrorMessage = createResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<IdentityRole<Guid>>> UpdateRoleAsync(Guid id, string updatedRole)
	{
		IdentityRole<Guid> roleToUpdate = await roleManager.FindByIdAsync(id.ToString());

		if (roleToUpdate is null)
		{
			return new ResponseDTO<IdentityRole<Guid>> { Success = false, ErrorMessage = "Kunne ikke redigere rollen, da rollen ikke findes i systemet.", ErrorType = ErrorType.EntityNotFound };
		}

		roleToUpdate.Name = updatedRole;

		IdentityResult updateResult = await roleManager.UpdateAsync(roleToUpdate);

		return updateResult.Succeeded
			? new ResponseDTO<IdentityRole<Guid>> { Success = true, Content = roleToUpdate }
			: new ResponseDTO<IdentityRole<Guid>> { Success = false, ErrorMessage = updateResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<IdentityRole<Guid>>> DeleteRoleAsync(string role)
	{
		IdentityRole<Guid> roleToDelete = await roleManager.FindByNameAsync(role);

		if (roleToDelete is null)
		{
			return new ResponseDTO<IdentityRole<Guid>> { Success = false, ErrorMessage = "Kunne ikke slette rollen, da navnet på rollen ikke findes i systemet.", ErrorType = ErrorType.EntityNotFound };
		}

		IdentityResult deleteResult = await roleManager.DeleteAsync(roleToDelete);

		return deleteResult.Succeeded
			? new ResponseDTO<IdentityRole<Guid>> { Success = true, Content = roleToDelete }
			: new ResponseDTO<IdentityRole<Guid>> { Success = false, ErrorMessage = deleteResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UserManagerError };
	}

	public async Task<ResponseDTO<Member>> RevokeMemberByIdAsync(Guid id)
	{
		Member memberToRevoke = await userManager.FindByIdAsync(id.ToString());

		if (memberToRevoke is null)
		{
			return new ResponseDTO<Member> { Success = false, ErrorMessage = "Kunne ikke finde dette medlem. Prøv igen eller kontakt en administrator.", ErrorType = ErrorType.EntityNotFound };
		}

		memberToRevoke.RefreshToken = null;
		memberToRevoke.RefreshTokenExpiryTime = null;

		IdentityResult revokeResult = await userManager.UpdateAsync(memberToRevoke);

		return revokeResult.Succeeded
			? new ResponseDTO<Member> { Success = true, Content = memberToRevoke }
			: new ResponseDTO<Member> { Success = false, ErrorMessage = "Kunne ikke logge bruger ud. Prøv igen eller kontakt en administrator.", ErrorType = ErrorType.UnableToRevokeMember };
	}

	public async Task<ResponseDTO<Member>> RevokeAllMembersAsync(CancellationToken cancellationToken = default)
	{
		List<Member> membersToRevoke;

		try
		{
			membersToRevoke = await userManager.Users.ToListAsync(cancellationToken);
		}
		catch (OperationCanceledException ex)
		{
			return new ResponseDTO<Member> { Success = false, ErrorMessage = "Din anmodning blev annulleret.", ExceptionMessage = ex.Message, InnerExceptionMessage = ex.InnerException?.Message, ErrorType = ErrorType.CancellationTokenRequested };
		}

		foreach (Member memberToRevoke in membersToRevoke)
		{
			memberToRevoke.RefreshToken = null;
			memberToRevoke.RefreshTokenExpiryTime = null;

			IdentityResult revokeResult = await userManager.UpdateAsync(memberToRevoke);

			if (!revokeResult.Succeeded)
			{
				return new ResponseDTO<Member> { Success = false, ErrorMessage = revokeResult.Errors.FirstOrDefault()?.Description, ErrorType = ErrorType.UnableToRevokeMember };
			}
		}

		return new ResponseDTO<Member> { Success = true };
	}
}