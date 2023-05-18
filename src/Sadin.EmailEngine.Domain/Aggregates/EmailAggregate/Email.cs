using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate.ValueObjects;

namespace Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

public record class Email
{
    private Email(string recipientName, string recipientEmailAddress, string subject, string body, bool isHtmlBody, List<EmailAttachment>? attachments)
    {
        RecipientName = recipientName ?? throw new ArgumentNullException(nameof(recipientName));
        RecipientEmailAddress = recipientEmailAddress ?? throw new ArgumentNullException(nameof(recipientEmailAddress));
        Subject = subject ?? throw new ArgumentNullException(nameof(subject));
        Body = body ?? throw new ArgumentNullException(nameof(body));
        IsHtmlBody = isHtmlBody;
        Attachments = attachments ?? new List<EmailAttachment>();
    }
    public string RecipientName { get; private set; }
    public string RecipientEmailAddress { get; private set; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public bool IsHtmlBody { get; private set; }
    public List<EmailAttachment> Attachments { get; private set; }

    public static Email Create(string recipientName, string recipientEmailAddress, string subject, string body, bool isHtmlBody, List<EmailAttachment>? attachments)
    {
        return new Email(recipientName, recipientEmailAddress, subject, body, isHtmlBody, attachments);
    }
}