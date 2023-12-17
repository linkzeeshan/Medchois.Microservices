using Microsoft.AspNetCore.Identity;

namespace UserManagementServices.Domain.Dtos.APIDtos
{
    public class CreateUserResponse
    {
        public string Token { get; set; }
        public IdentityUser User { get; set; }
    }
}
