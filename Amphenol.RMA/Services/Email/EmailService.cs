using AIO.MailDispatch.Abstractions;
using AIO.MailDispatch.Models;
using Amphenol.RMA.Services.Email.Templates;
using Amphenol.RMA.ViewModels;
using Humanizer;
using iTextSharp.tool.xml.html;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Services.WebApi.Exceptions;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Amphenol.RMA.Services.Email
{
    public interface IEmailService
    {
        public Task SendApprovalRequestNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default);
        public Task SendApproveNotification(string title, string subtitle, string message, EmailAddress representativeEmail, EmailAddress managerEmail, EmailAttachment attachment, CancellationToken cancellationToken = default);
        public Task SendRejectNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default);
        public Task SendCustomerNotification(string title, string subtitle, string message, EmailAddress recipient, EmailAttachment attachment, CancellationToken cancellationToken = default);
    }
    internal class EmailService(IMailSender mailSender, IEmailTemplateRenderer templateRenderer) : IEmailService
    {
        private readonly string notificationTemplatePath = "/Views/Shared/_EmailInternalNotification.cshtml";
        public async Task SendApprovalRequestNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Approval Required", htmlBody: htmlMessage);

#if DEBUG
            emailMessage.To.Add(new EmailAddress("jhernandez@amphenol-aio.com"));
#else
                emailMessage.To.Add(recipient);
#endif

                await mailSender.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task SendApproveNotification(string title, string subtitle, string message, EmailAddress representativeEmail, EmailAddress managerEmail, EmailAttachment attachment, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Request Approved", htmlBody: htmlMessage);
                emailMessage.Attachments.Add(attachment);
#if DEBUG
            emailMessage.To.Add(new EmailAddress("jhernandez@amphenol-aio.com"));
#else
                emailMessage.To.Add(representativeEmail);
                emailMessage.Cc.Add(managerEmail);
#endif

                await mailSender.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task SendCustomerNotification(string title, string subtitle, string message, EmailAddress recipient, EmailAttachment attachment, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Approved", htmlBody: htmlMessage);
                emailMessage.Attachments.Add(attachment);

#if DEBUG
            emailMessage.To.Add(new EmailAddress("jhernandez@amphenol-aio.com"));
#else
                emailMessage.To.Add(recipient);
#endif

                await mailSender.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task SendRejectNotification(string title, string subtitle, string message, EmailAddress recipient, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlMessage = await templateRenderer.RenderAsync(notificationTemplatePath, new InternalEmailNotificationViewModel()
                {
                    Title = title,
                    Subtitle = subtitle,
                    Message = message,
                });

                var emailMessage = EmailMessage.Create(subject: "RMA Request Rejected", htmlBody: htmlMessage);

#if DEBUG
            emailMessage.To.Add(new EmailAddress("jhernandez@amphenol-aio.com"));
#else
                emailMessage.To.Add(recipient);
#endif

                await mailSender.SendAsync(emailMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
