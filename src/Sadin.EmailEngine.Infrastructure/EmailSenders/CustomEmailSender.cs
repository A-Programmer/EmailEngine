using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
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
    public async Task SendAsync(Email email)
    {
        // TODO : Remove this line:
        Console.WriteLine("Sending email to {0}", email.RecipientName);
        
        using (var client = new SmtpClient())
        {
            try
            {
                MimeMessage mailMessage = CreateEmailMessage(email);
                await client.ConnectAsync(_customEmailProviderConfigurations.SmtpServer, _customEmailProviderConfigurations.SmtpPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_customEmailProviderConfigurations.UserName, _customEmailProviderConfigurations.Password);
                await client.SendAsync(mailMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
    
    private MimeMessage CreateEmailMessage(Email message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Sadin.DEV", _customEmailProviderConfigurations.From));
        emailMessage.To.Add(new MailboxAddress(message.RecipientName, message.RecipientEmailAddress));
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = message.Body };

        if (message.Attachments != null && message.Attachments.Any())
        {
            byte[] fileBytes;
            foreach (var attachment in message.Attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attachment.Attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                bodyBuilder.Attachments.Add(attachment.Attachment.FileName, fileBytes, ContentType.Parse(attachment.Attachment.ContentType));
            }
        }
        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }
}