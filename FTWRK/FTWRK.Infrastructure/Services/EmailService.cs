using FTWRK.Application.Common.Interfaces;
using FTWRK.Application.Common.Models;
using FTWRK.Infrastructure.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FTWRK.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfiguration _smtpConfiguration;

        public EmailService(IOptions<SmtpConfiguration> smtpOptions)
        {
            _smtpConfiguration = smtpOptions.Value;
        }

        public async Task<bool> SendAsync(EmailMessage message)
        {
            var email = CreateMessage(message);

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpConfiguration.SmtpServer, _smtpConfiguration.Port, true);
                await client.AuthenticateAsync(_smtpConfiguration.UserName, _smtpConfiguration.Password);

                var res = await client.SendAsync(email);

                await client.DisconnectAsync(true);

                return true;
            }
        }

        private MimeMessage CreateMessage(EmailMessage message)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_smtpConfiguration.From.Name, _smtpConfiguration.From.Email));
            email.To.Add(new MailboxAddress("", message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Body
            };

            return email;
        }
    }
}
