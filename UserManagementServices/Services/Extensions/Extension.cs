using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using UserManagementServices.Domain.Dtos.EmailDtos;

namespace UserManagementServices.Services.Extensions
{
    public static class Extension
    {
        public static Message SendEmailConfirmation(IdentityUser user, string token, string baseUrl)
        {
            var confirmationLink = $"{baseUrl}/ConfirmEmail?email={user.Email}&token={token}";
            var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink!);
            return message;
        }
        public static Message SendEmailForGotPassword(IdentityUser user, string token, string baseUrl)
        {
            var forgotPasswordlink = $"{baseUrl}/ResetPassword?email={user.Id}&token={token}";
            var message = new Message(new string[] { user.Email! }, "Forgot Password Link", forgotPasswordlink);
            return message;
        } 
        public static Message SendEmailLoginOtp(IdentityUser user, string token)
        {
            var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
            return message;
        }
    }
}
