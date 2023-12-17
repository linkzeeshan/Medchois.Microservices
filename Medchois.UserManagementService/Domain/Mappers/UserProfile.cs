using Microsoft.AspNetCore.Identity;
using Medchois.UserManagementService.Domain.Dtos.UserDtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Medchois.UserManagementService.Domain.Profiles
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
