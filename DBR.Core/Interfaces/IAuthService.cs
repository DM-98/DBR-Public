using DBR.Core.Domain;
using DBR.Core.DTOs.Inputs;
using DBR.Core.DTOs.Outputs;
using Microsoft.AspNetCore.Identity;

namespace DBR.Core.Interfaces;

public interface IAuthService
{
	Task<ResponseDTO<Member>> RegisterAsync(RegisterInputModel registerInputModel);

	Task<ResponseDTO<Tuple<string, string>>> LoginAsync(LoginInputModel loginInputModel);

	Task<ResponseDTO<Tuple<string, string>>> UpdateMemberAsync(Member updatedMember);

	Task<ResponseDTO<Tuple<string, string>>> RefreshTokenAsync(Tuple<string, string> accessRefreshTokensToRefresh);

	Task<ResponseDTO<IdentityRole<Guid>>> CreateRoleAsync(string role);

	Task<ResponseDTO<IdentityRole<Guid>>> UpdateRoleAsync(Guid id, string updatedRole);

	Task<ResponseDTO<IdentityRole<Guid>>> DeleteRoleAsync(string role);

	Task<ResponseDTO<Member>> RevokeMemberByIdAsync(Guid id);

	Task<ResponseDTO<Member>> RevokeAllMembersAsync(CancellationToken cancellationToken = default);
}