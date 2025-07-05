
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using NuGet.Protocol.Plugins;
using QuizSystem_backend.DTOs.UserEmailDto;
using System.Net.Mail;

namespace QuizSystem_backend.services.MailServices
{
    public class SendMailService : IMailService
    {
        private readonly MailSettings _mailSetting;
        private readonly ILogger _logger;

        public SendMailService(IOptions<MailSettings> mailSetting, ILogger<SendMailService> logger)
        {
            _mailSetting = mailSetting.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(MailContent template, List<UserEmailDto> listUser)
        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
               
                await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);

            
                foreach (var user in listUser)
                {
                    
                    var email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
                    email.From.Add(new MailboxAddress(_mailSetting.Displayname, _mailSetting.Mail));
                    email.To.Add(new MailboxAddress(user.Email, user.Email));
                    email.Subject = template.Subject;

                    var builder = new BodyBuilder { HtmlBody = template.Body };
                    email.Body = builder.ToMessageBody();

                    try
                    {
                        // Gửi từng mail
                        await smtp.SendAsync(email);
                    }
                    catch (Exception sendEx)
                    {
                        _logger.LogError(sendEx, "Lỗi gửi mail đến {Email}", user.Email);

                        // Lưu lại email bị lỗi
                        try
                        {
                            var saveDir = Path.Combine(Directory.GetCurrentDirectory(), "mailssave");
                            if (!Directory.Exists(saveDir))
                                Directory.CreateDirectory(saveDir);

                            var filePath = Path.Combine(saveDir, $"{Guid.NewGuid():N}.eml");
                            await email.WriteToAsync(filePath);
                            _logger.LogInformation("Đã lưu email lỗi tại {Path}", filePath);
                        }
                        catch (Exception ioEx)
                        {
                            _logger.LogError(ioEx, "Không thể lưu email lỗi cho {Email}", user.Email);
                        }

                        // Tiếp tục gửi cho user kế tiếp
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Lỗi kết nối hoặc xác thực SMTP");
                throw;  // tuỳ yêu cầu bạn có thể rethrow hoặc swallow
            }
            finally
            {
                // 3. Luôn ngắt kết nối
                if (smtp.IsConnected)
                    await smtp.DisconnectAsync(true);
            }
        }

       
    }
}
