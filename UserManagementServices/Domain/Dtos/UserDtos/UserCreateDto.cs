using System.ComponentModel.DataAnnotations;

namespace UserManagementServices.Domain.Dtos.UserDtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Name is Email")]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Name is Password")]
        public string? Password { get; set; }
        public string[] UserRoles { get; set; } = new string[0];
    }
}
