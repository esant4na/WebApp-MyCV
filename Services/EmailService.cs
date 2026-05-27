using MailKit.Net.Smtp;
using MimeKit;
using WebApp_MyCV.Models;

namespace WebApp_MyCV.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(ContactFormModel model)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(
                    _configuration["EmailSettings:SenderEmail"]));

            email.To.Add(
                MailboxAddress.Parse(
                    _configuration["EmailSettings:ReceiverEmail"]));

            email.Subject = "Nuevo mensaje desde MyCV";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                    <h2>Nuevo mensaje de contacto</h2>

                    <p><strong>Nombre:</strong> {model.Name}</p>

                    <p><strong>Email:</strong> {model.Email}</p>

                    <p><strong>Mensaje:</strong></p>

                    <p>{model.Message}</p>
                "
            };

            using var smtp = new SmtpClient
            {
                Timeout = 20000
            };

            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.SslOnConnect);

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
