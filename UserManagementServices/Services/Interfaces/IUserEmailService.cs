using UserManagementServices.Domain.Dtos.EmailDtos;

namespace UserManagementServices.Services.Interfaces
{
    public interface IUserEmailService
    {
        Task SendEmailAsyc(Message message);
    }
}
