using Microsoft.AspNetCore.Identity;
using UserManagementServices.Domain.Dtos.APIDtos;
using UserManagementServices.Domain.Dtos.UserDtos;

namespace UserManagementServices.Domain.IRepository
{
    public interface IUserManagementRepo
    {
        Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user);
        Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token);
        Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginDto login);
        Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email);
        Task<ApiResponse<string>> ForgotPasswordAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto model);
        Task<ApiResponse<ResetPasswordDto>> ResetPasswordAsync(string token, string email);
        Task<IdentityUser> GetUserByEmailAsync(string email);
    }
}
