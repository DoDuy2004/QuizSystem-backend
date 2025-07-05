using QuizSystem_backend.DTOs.UserEmailDto;

namespace QuizSystem_backend.services.MailServices
{
    public interface IMailService
    {
        Task SendEmailAsync(MailContent template, List<UserEmailDto> listUser);
    }
}
