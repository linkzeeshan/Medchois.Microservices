using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Medchois.UserManagementService.Domain.Dtos.APIDtos;
using Medchois.UserManagementService.Domain.Dtos.UserDtos;

namespace Medchois.UserManagementService.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user);
        Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token);
        Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginDto login);
        Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto model);
        Task<ApiResponse<List<string>>> AssignRoleToUserAsync(IEnumerable<string> roles, IdentityUser user);
        Task<IdentityUser> GetUserByEmailAsync(string email);
        Task<IdentityUser> GetUserByIdAsync(string id);

    }
}
