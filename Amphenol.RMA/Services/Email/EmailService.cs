using AIO.MailDispatch.Abstractions;
using AIO.MailDispatch.Models;
using Amphenol.RMA.Services.Email.Templates;
using Amphenol.RMA.ViewModels;
using Humanizer;
using iTextSharp.tool.xml.html;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.WebApi.Exceptions;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Amphenol.RMA.Services.Email
{
    public interface IEmailService
    {
        public Task SendApprovalRequestNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default);
        public Task SendApproveNotification(string title, string subtitle, string message, IReadOnlyCollection<EmailAddress> recipients, EmailAttachment attachment, CancellationToken cancellationToken = default);
        public Task SendRejectNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default);
        public Task SendCustomerNotification(string title, string subtitle, string message, EmailAddress recipient, EmailAttachment attachment, CancellationToken cancellationToken = default);
    }
    internal class EmailService(IMailSender mailSender, IEmailTemplateRenderer templateRenderer,
        ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
    {
        private readonly string notificationTemplatePath = "/Views/Shared/_EmailInternalNotification.cshtml";
        public async Task SendApprovalRequestNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default)
        {
            if (SuppressTestDelivery("ApprovalRequest", subtitle)) return;
            try
            {
                var deliveryRecipient = GetDebugRecipient() ?? recipient;
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Approval Required", htmlBody: htmlMessage);

                emailMessage.To.Add(deliveryRecipient);

                await mailSender.SendAsync(emailMessage, cancellationToken);
                logger.LogInformation("RMA email sent successfully. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "ApprovalRequest", string.Join(", ", emailMessage.To.Select(x => x.ToString())), subtitle);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RMA email failed. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "ApprovalRequest", recipient?.ToString(), subtitle);
            }
        }

        public async Task SendApproveNotification(string title, string subtitle, string message, IReadOnlyCollection<EmailAddress> recipients, EmailAttachment attachment, CancellationToken cancellationToken = default)
        {
            if (SuppressTestDelivery("Approval", subtitle)) return;
            try
            {
                var debugRecipient = GetDebugRecipient();
                var deliveryRecipients = new List<EmailAddress>();
                if (debugRecipient != null)
                    deliveryRecipients.Add(debugRecipient);
                else if (recipients != null)
                    deliveryRecipients.AddRange(recipients.Where(x => x != null));
                var uniqueRecipients = deliveryRecipients
                    .GroupBy(x => x.ToString(), StringComparer.OrdinalIgnoreCase)
                    .Select(x => x.First())
                    .ToList();
                if (uniqueRecipients.Count == 0)
                {
                    logger.LogWarning("RMA approval completion email skipped because no internal recipients were resolved. Rma={Rma}", subtitle);
                    return;
                }

                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Request Approved", htmlBody: htmlMessage);
                emailMessage.Attachments.Add(attachment);
                foreach (var recipient in uniqueRecipients)
                    emailMessage.To.Add(recipient);

                await mailSender.SendAsync(emailMessage, cancellationToken);
                logger.LogInformation("RMA email sent successfully. Type={EmailType}, Recipients={Recipients}, Rma={Rma}",
                    "Approval", string.Join(", ", emailMessage.To.Select(x => x.ToString())), subtitle);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RMA email failed. Type={EmailType}, Recipients={Recipients}, Rma={Rma}",
                    "Approval", string.Join(", ", recipients?.Select(x => x.ToString()) ?? Enumerable.Empty<string>()), subtitle);
            }

        }

        public async Task SendCustomerNotification(string title, string subtitle, string message, EmailAddress recipient, EmailAttachment attachment, CancellationToken cancellationToken = default)
        {
            if (SuppressTestDelivery("CustomerApproval", subtitle)) return;
            try
            {
                var deliveryRecipient = GetDebugRecipient() ?? recipient;
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Approved", htmlBody: htmlMessage);
                emailMessage.Attachments.Add(attachment);

                emailMessage.To.Add(deliveryRecipient);

                await mailSender.SendAsync(emailMessage, cancellationToken);
                logger.LogInformation("RMA email sent successfully. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "CustomerApproval", string.Join(", ", emailMessage.To.Select(x => x.ToString())), subtitle);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RMA email failed. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "CustomerApproval", recipient?.ToString(), subtitle);
            }
        }

        public async Task SendRejectNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default)
        {
            if (SuppressTestDelivery("Rejection", subtitle)) return;
            try
            {
                var deliveryRecipient = GetDebugRecipient() ?? recipient;
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Request Rejected", htmlBody: htmlMessage);

                emailMessage.To.Add(deliveryRecipient);

                await mailSender.SendAsync(emailMessage, cancellationToken);
                logger.LogInformation("RMA email sent successfully. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "Rejection", string.Join(", ", emailMessage.To.Select(x => x.ToString())), subtitle);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RMA email failed. Type={EmailType}, Recipient={Recipient}, Rma={Rma}",
                    "Rejection", recipient?.ToString(), subtitle);
            }
        }

        private bool SuppressTestDelivery(string emailType, string subtitle)
        {
            var reason = GetTestHostReason();
            if (reason == null) return false;

            logger.LogInformation("RMA email suppressed for test execution. Type={EmailType}, Reason={Reason}, Rma={Rma}",
                emailType, reason, subtitle);
            return true;
        }

        private EmailAddress? GetDebugRecipient()
        {
#if DEBUG
            return ConfiguredDebugRecipient();
#else
            if (Debugger.IsAttached)
                return ConfiguredDebugRecipient();

            return null;
#endif
        }

        private EmailAddress ConfiguredDebugRecipient()
        {
            var configuredAddress = configuration["Email:DebugRecipient"];
            return new EmailAddress(string.IsNullOrWhiteSpace(configuredAddress)
                ? "jhernandez@amphenol-aio.com"
                : configuredAddress.Trim());
        }

        private static string? GetTestHostReason()
        {
            using var process = Process.GetCurrentProcess();
            var processName = process.ProcessName;
            if (processName.Contains("testhost", StringComparison.OrdinalIgnoreCase)
                || processName.Contains("vstest", StringComparison.OrdinalIgnoreCase))
                return $"Test process ({processName})";

            if (AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                assembly.GetName().Name is string name
                && (name.StartsWith("xunit", StringComparison.OrdinalIgnoreCase)
                    || name.StartsWith("nunit", StringComparison.OrdinalIgnoreCase)
                    || name.StartsWith("Microsoft.TestPlatform", StringComparison.OrdinalIgnoreCase))))
                return "Test runner loaded";

            return null;
        }
    }
}
