


using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using NuGet.Protocol.Plugins;
using QuizSystem_backend.DTOs.UserEmailDto;


namespace QuizSystem_backend.services.MailServices
{
    public class SendMailService : IEmailSender
    {
        private readonly MailSettings _mailSetting;
        private readonly ILogger _logger;

        public SendMailService(IOptions<MailSettings> mailSetting, ILogger<SendMailService> logger)
        {
            _mailSetting = mailSetting.Value;
            _logger = logger;
        }


        public async Task SendEmailAsync(string mail, string subject, string htmlMessage)

        {
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {

                await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);



                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
                email.From.Add(new MailboxAddress(_mailSetting.Displayname, _mailSetting.Mail));
                email.To.Add(new MailboxAddress(mail, mail));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlMessage };
                email.Body = builder.ToMessageBody();

                try
                {
                    // Gửi từng mail
                    await smtp.SendAsync(email);
                }
                catch (Exception sendEx)
                {
                    _logger.LogError(sendEx, "Lỗi gửi mail đến {Email}", mail);

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
                        _logger.LogError(ioEx, "Không thể lưu email lỗi cho {Email}", mail);
                    }

                    // Tiếp tục gửi cho user kế tiếp
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
