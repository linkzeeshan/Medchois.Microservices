﻿using Microsoft.AspNetCore.Identity;
using UserManagementServices.Domain.Dtos.APIDtos;
using UserManagementServices.Domain.Dtos.UserDtos;

namespace UserManagementServices.Services.Interfaces
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
