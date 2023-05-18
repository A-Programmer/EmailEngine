using System.Text;
using Sadin.EmailEngine.Domain.Abstractions;
using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

namespace Sadin.EmailEngine.Application.Builders.EmailBuilders;

public class ContactMessageCreatedEmailBuilder : EmailCreator
{
    public override Email CreateEmail(string recipientName, string recipientEmailAddress, string subject, string message, byte[] attachments)
    {
        // TODO : HeaderCreator, BodyCreator, FooterCreator must be implemented.
        
        StringBuilder emailText = new();
        emailText.AppendLine($"<p>Hi dear {recipientName}</p>\n");
        emailText.AppendLine("<p>We received your message and we will reply ASAP.</p>\n");
        
        _body = _body
            .Replace("{body}", emailText.ToString())
            .Replace("{footerText}", "Sadin Co, Tehran, Iran, 49718.")
            .Replace("{unsubscriptionUrl}", "https://sadin.dev/unsubscribe?id=test");
        
        return Email.Create(recipientName, recipientEmailAddress, subject, _body, true, null);
    }
}