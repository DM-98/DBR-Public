using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DBR.Core.Helpers;

public static class AuthHelper
{
	public static JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration)
	{
		SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
		SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
		JwtSecurityToken jwtSecurityToken = new(claims: claims, audience: configuration["JWT:Audience"], issuer: configuration["JWT:Issuer"], signingCredentials: signingCredentials, expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT:AccessTokenExpiryInMinutes"])));

		return jwtSecurityToken;
	}

	public static string GenerateRefreshToken()
	{
		byte[] randomNumber = new byte[64];
		using RandomNumberGenerator rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);

		return Convert.ToBase64String(randomNumber);
	}

	public static ClaimsPrincipal? TryGetClaimsPrincipal(string accessToken, IConfiguration configuration)
	{
		SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

		TokenValidationParameters tokenValidationParameters = new()
		{
			ValidateIssuerSigningKey = true,
			ValidateLifetime = false,
			ValidateAudience = true,
			ValidateIssuer = true,
			IssuerSigningKey = symmetricSecurityKey,
			ValidAudience = configuration["JWT:Audience"],
			ValidIssuer = configuration["JWT:Issuer"],
			ClockSkew = TimeSpan.Zero
		};

		JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
		ClaimsPrincipal? principal;

		principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

		if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
		{
			return null;
		}

		return principal;
	}

	public static IEnumerable<Claim>? ParseClaimsFromJWT(string jwt)
	{
		List<Claim> claims = new();
		string payload = jwt.Split('.')[1];
		byte[] jsonBytes = ParseBase64WithoutPadding(payload);
		Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;

		ExtractRolesFromJWT(claims, keyValuePairs);
		claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));

		return claims;
	}

	private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
	{
		keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);

		if (roles is not null)
		{
			string[] parsedRoles = roles.ToString()!.Trim().TrimStart('[').TrimEnd(']').Split(',');

			if (parsedRoles.Length > 1)
			{
				foreach (string parsedRole in parsedRoles)
				{
					claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
				}
			}
			else
			{
				claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));
			}

			keyValuePairs.Remove(ClaimTypes.Role);
		}
	}

	private static byte[] ParseBase64WithoutPadding(string base64)
	{
		switch (base64.Length % 4)
		{
			case 2:
				base64 += "==";
				break;

			case 3:
				base64 += "=";
				break;
		}

		return Convert.FromBase64String(base64);
	}
}