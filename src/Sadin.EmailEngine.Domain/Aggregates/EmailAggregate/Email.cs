using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate.ValueObjects;

namespace Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

public record class Email
{
    private Email(string recipient, string subject, string body, List<EmailAttachment>? attachments)
    {
        Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        Body = body ?? throw new ArgumentNullException(nameof(body));
        Attachments = attachments ?? new List<EmailAttachment>();
    }
    public string Recipient { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public List<EmailAttachment> Attachments { get; private set; }

    public static Email Create(string receiver, string subject, string body, List<EmailAttachment>? attachments)
    {
        return new Email(receiver, subject, body, attachments);
    }
}