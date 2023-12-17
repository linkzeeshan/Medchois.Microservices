using Microsoft.AspNetCore.Identity;

namespace Medchois.UserManagementService.Domain.Dtos.APIDtos
{
    public class CreateUserResponse
    {
        public string Token { get; set; }
        public IdentityUser User { get; set; }
    }
}
