using Microsoft.AspNetCore.Identity;
using Medchois.UserManagementService.Domain.Dtos.APIDtos;
using Medchois.UserManagementService.Domain.Dtos.UserDtos;
using Medchois.UserManagementService.Services.Interfaces;
using Medchois.UserManagementService.Domain.IRepository;

namespace Medchois.UserManagementService.Infrastructure.Repositories
{
    public class UserManagementRepo : IUserManagementRepo
    {
        private readonly IUserService _userService;

        public UserManagementRepo(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token)
        {
            return await _userService.ConfirnmEmailAsync(email, token);
        }

        public async Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto user)
        {
            //user is created but not activated
            return await _userService.CreateAsync(user);
        }

        public Task<ApiResponse<string>> ForgotPasswordAsync(string email)
        {
            return _userService.ForgotPasswordAsync(email);
        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        public async Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginDto login)
        {
            return await _userService.LoginAsync(login);
        }

        public async Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email)
        {
            return await _userService.LoginWithOTPAsync(code, email);
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto model)
        {
            return await _userService.ResetPasswordAsync(model);
        }

        public async Task<ApiResponse<ResetPasswordDto>> ResetPasswordAsync(string token, string id)
        {
            if (token == null) { throw new ArgumentNullException("token"); }
            if (id == null) { throw new ArgumentNullException("id"); }
            var user = await _userService.GetUserByIdAsync(id);
            //token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var model = new ResetPasswordDto { Email = user.Email, Token = token };
            return new ApiResponse<ResetPasswordDto> { Success = true, Data = model };
        }
    }
}
