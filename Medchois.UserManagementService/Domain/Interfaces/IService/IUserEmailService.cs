using Medchois.UserManagementService.Domain.Dtos.EmailDtos;

namespace Medchois.UserManagementService.Services.Interfaces
{
    public interface IUserEmailService
    {
        Task SendEmailAsyc(Message message);
    }
}
