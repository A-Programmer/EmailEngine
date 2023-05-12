using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Sadin.EmailEngine.Domain;
using Sadin.EmailEngine.Domain.Abstractions;
using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;
using Sadin.EmailEngine.Infrastructure.Models;

namespace Sadin.EmailEngine.Infrastructure.EmailSenders;

public sealed class CustomEmailSender : IEmailSender
{
    private readonly CustomEmailProviderConfigurations _customEmailProviderConfigurations;

    public CustomEmailSender(IOptions<CustomEmailProviderConfigurations> emailConfigurations)
    {
        _customEmailProviderConfigurations = emailConfigurations.Value;
    }
    public void Send(EmailProvider provider, Email email)
    {
        // TODO : Remove this line:
        Console.WriteLine("Sending email to {0}", email.Recipient);
        
        using (var client = new SmtpClient())
        {
            client.Host = _customEmailProviderConfigurations.SmtpServer;
            client.Port = _customEmailProviderConfigurations.SmtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = _customEmailProviderConfigurations.EnableSsl;
            client.Credentials = new NetworkCredential(_customEmailProviderConfigurations.UserName, _customEmailProviderConfigurations.Password);
            using (var message = new MailMessage(
                       from: new MailAddress(_customEmailProviderConfigurations.From,"Sadin.DEV"),
                       to: new MailAddress(email.Recipient)
                   ))
            {

                message.Subject = email.Subject;
                message.Body = email.Body;

                if (email.Attachments.Any())
                {
                    int i = 0;
                    foreach (var attachment in email.Attachments)
                    {
                        message.Attachments.Add(new(new MemoryStream(attachment.Attachment),$"Attachment-{i}"));
                    }
                }

                client.Send(message);
            }
        }
    }
}