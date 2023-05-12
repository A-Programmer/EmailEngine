using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

namespace Sadin.EmailEngine.Domain.Abstractions;

public interface IEmailSender
{
    void Send(EmailProvider provider, Email email);
}