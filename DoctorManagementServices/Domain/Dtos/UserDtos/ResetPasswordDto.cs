using System.ComponentModel.DataAnnotations;

namespace UserManagementServices.Domain.Dtos.UserDtos
{
    /// <summary>
    /// User Reser Password view Model
    /// </summary>
    public class ResetPasswordDto
    {
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password are not match")]
        public string? ConfirmPassword { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
    }
}
