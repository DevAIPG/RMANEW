using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using iTextSharp.tool.xml.html;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Services.WebApi.Exceptions;
using MimeKit;

namespace Amphenol.RMA.Services.Email
{
    public class EmailSettings
    {
        public string SenderAccount { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public List<string> Bcc { get; set; }
    }
    public interface IEmailService
    {
        EmailSettings EmailSettings { get; }

        public Task SendEmail(string subject, string body, List<string> recipients, List<string> bcc);
    }
    internal class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
    {
        public EmailSettings EmailSettings { get; } = emailSettings.Value;

        public async Task SendEmail(string subject, string body, List<string> recipients, List<string> bcc)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(subject);
            ArgumentException.ThrowIfNullOrWhiteSpace(body);
            if (recipients is null || !recipients.Any())
            {
                throw new ArgumentNullException(nameof(recipients));
            }

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(EmailSettings.SenderAccount, EmailSettings.SenderAccount));
            email.To.AddRange(recipients.Select(InternetAddress.Parse));

            if (bcc is not null && bcc.Any())
            {
                email.Bcc.AddRange(bcc.Select(InternetAddress.Parse));
            }
#if DEBUG
            email.To.Clear();
            email.Bcc.Clear();
            email.To.Add(InternetAddress.Parse("jhernandez@amphenol-aio.com"));
#endif

            email.Subject = subject;

            email.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(EmailSettings.SmtpServer, EmailSettings.Port, SecureSocketOptions.StartTls);

                //TODO: set password
                await client.AuthenticateAsync(EmailSettings.SenderAccount, "{PasswordPlaceHolder}");

                await client.SendAsync(email);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true);
                }
            }

        }
    }
}
