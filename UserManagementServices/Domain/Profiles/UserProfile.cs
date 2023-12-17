using Microsoft.AspNetCore.Identity;
using UserManagementServices.Domain.Dtos.UserDtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserManagementServices.Domain.Profiles
{
    public class UserProfile
    {
        public UserProfile() {
            // Source -> Target
            //CreateMap<IdentityUser, UserCreateDto>();
            //CreateMap<UserCreateDto, IdentityUser>();
        }
    }
}
