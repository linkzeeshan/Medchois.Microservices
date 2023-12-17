using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using UserManagementServices.Domain.Dtos.APIDtos;
using UserManagementServices.Domain.Dtos.EmailDtos;
using UserManagementServices.Domain.Dtos.UserDtos;
using UserManagementServices.Services.Interfaces;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace UserManagementServices.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly ILogger<UserService> _logger;
        private readonly IUserEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        #endregion
        #region Ctor
        public UserService(ILogger<UserService> logger, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration,
            IUserEmailService emailService, SignInManager<IdentityUser> signinManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _signinManager = signinManager;

        }

        public async Task<ApiResponse<List<string>>> AssignRoleToUserAsync(IEnumerable<string> roles, IdentityUser user)
        {
            var assignRole = new List<string>();
            try
            {

                foreach (var role in roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        if (!await _userManager.IsInRoleAsync(user, role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                        }

                    }
                }
                return new ApiResponse<List<string>> { Data = assignRole, Message = "Roles has  been assigned", StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<string>> { Data = assignRole, Message = "Roles are not assigned", StatusCode = StatusCodes.Status400BadRequest };
            }
        }
        #endregion

        #region Interface Implementation
        public async Task<ApiResponse<string>> ConfirnmEmailAsync(string email, string token)
        {
            try
            {
                IdentityUser user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        return new ApiResponse<string> { Success = true, Message = $"Email is confirmed successfully on {user.Email}", StatusCode = StatusCodes.Status201Created };

                    }

                }
                return new ApiResponse<string> { Success = false, Message = $"Email does not confirmed {user.Email}", StatusCode = StatusCodes.Status201Created };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = $"{ex.Message}", StatusCode = StatusCodes.Status201Created };
            }
        }

        public async Task<ApiResponse<CreateUserResponse>> CreateAsync(UserCreateDto registerUser)
        {
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExist != null)
            {
                var response = new ApiResponse<string>
                {
                    Success = false,
                    Message = "User Already Exist",
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            //Add user in database
            IdentityUser user = new()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                TwoFactorEnabled = true, //I have Enabled two factor authentication It will verify user on login leve
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                return new ApiResponse<CreateUserResponse> { Success = true, Message = $"User Fail Created: {result.Errors.FirstOrDefault().Description}", StatusCode = StatusCodes.Status201Created };

            }

            // Add Role
            await AssignRoleToUserAsync(registerUser.UserRoles, user);
            //Add token to verify email
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, user.Email);
            var baseURL = _configuration["EmailConfiguration:BaseURL"];
            var confirmationLink = $"{baseURL}/ConfirmEmail?email={user.Email}&token={token}";
            var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink!);
            await _emailService.SendEmailAsyc(message);

            return new ApiResponse<CreateUserResponse>
            {
                Success = true,
                Message = $"User created & Email sent to {user.Email} successfully",
                StatusCode = StatusCodes.Status201Created,
                Data = new CreateUserResponse { Token = token, User = user }
            };

        }

        public async Task<ApiResponse<string>> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var baseURL = _configuration["EmailConfiguration:BaseURL"];
                    var forgotPasswordlink = $"{baseURL}/ResetPassword?email={user.Id}&token={token}";
                    //var forgotPasswordlink = Url.Action("ResetPassword", "Authentication", new { token, email = user.Email }, Request.Scheme);
                    var message = new Message(new string[] { user.Email! }, "Forgot Password Link", forgotPasswordlink);
                    await _emailService.SendEmailAsyc(message);
                    return new ApiResponse<string> { Success = true, Message = $"We have sent to OTP on your Email {user.Email} successfully", StatusCode = StatusCodes.Status201Created };

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string> { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status400BadRequest };

            }
            return new ApiResponse<string> { Success = false, Message = $"Coludn't send link to email, Please try agian", StatusCode = StatusCodes.Status400BadRequest };

        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }
        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<ApiResponse<LoginOTPResponse>> LoginAsync(LoginDto login)
        {
            //chekcing user
            var user = await _userManager.FindByEmailAsync(login.Email);
            var authClaim = new List<Claim>();
            //checking password
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                //checking claim
                authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                //checking and add role in the list
                var userRoles = await _userManager.GetRolesAsync(user);
                //generate token with claim
                foreach (var role in userRoles)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }
                //Implementing  two factor authentication in case of Enabled two factor
                if (user.TwoFactorEnabled)
                {
                    //SignOut user
                    await _signinManager.SignOutAsync();
                    await _signinManager.PasswordSignInAsync(user, login.Password, false, true);
                    //sending OTP to user email for varification
                    //Add token to verify email
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
                    await _emailService.SendEmailAsyc(message);
                    return new ApiResponse<LoginOTPResponse>
                    {
                        Success = true,
                        Message = $"We have sent to OTP on your Email {user.Email} successfully",
                        StatusCode = StatusCodes.Status201Created,
                        Data = new LoginOTPResponse { Token = token, expiration = new DateTime() }
                    };

                }


            }
            else
            {
                return new ApiResponse<LoginOTPResponse>
                {
                    Success = true,
                    Message = $"Please enter correct email & password.",
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //return token as expected
            var jwtToken = GetToken(authClaim);
            return new ApiResponse<LoginOTPResponse> { Success = true, Message = $"Account is created successfully", StatusCode = StatusCodes.Status201Created, Data = new LoginOTPResponse { Token = new JwtSecurityTokenHandler().WriteToken(jwtToken), expiration = jwtToken.ValidTo } };

        }

        public async Task<ApiResponse<LoginOTPResponse>> LoginWithOTPAsync(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            try
            {
                var signin = await _signinManager.TwoFactorSignInAsync("Email", code, false, false);
                if (signin.Succeeded)
                {
                    if (user != null)
                    {
                        //checking claim
                        var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                        //checking and add role in the list
                        var userRoles = await _userManager.GetRolesAsync(user);
                        //generate token with claim
                        foreach (var role in userRoles)
                        {
                            authClaim.Add(new Claim(ClaimTypes.Role, role));
                        }
                        //return token as expected
                        var jwtToken = GetToken(authClaim);
                        return new ApiResponse<LoginOTPResponse> { Success = true, Message = $"Account is created successfully", StatusCode = StatusCodes.Status201Created, Data = new LoginOTPResponse { Token = new JwtSecurityTokenHandler().WriteToken(jwtToken), expiration = jwtToken.ValidTo } };

                    }

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginOTPResponse> { Success = true, Message = $"{ex.Message}", StatusCode = StatusCodes.Status400BadRequest, Data = new LoginOTPResponse() };

            }
            return new ApiResponse<LoginOTPResponse> { Success = true, Message = $"Invalid Code", StatusCode = StatusCodes.Status400BadRequest, Data = new LoginOTPResponse() };


        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassowrd = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPassword.Token)), resetPassword.Password);
                if (!resetPassowrd.Succeeded)
                {
                    return new ApiResponse<string> { Success = true, Message = $"Could not reset password please try again", StatusCode = StatusCodes.Status412PreconditionFailed };

                }

                return new ApiResponse<string> { Success = true, Message = $"Password has been changed", StatusCode = StatusCodes.Status201Created };


            }
            return new ApiResponse<string> { Success = true, Message = $"Could not reset password please try again", StatusCode = StatusCodes.Status412PreconditionFailed };

        }
        #endregion
        #region Method
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Issuer"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        #endregion

    }
}
