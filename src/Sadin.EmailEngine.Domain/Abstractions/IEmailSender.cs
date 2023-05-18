using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

namespace Sadin.EmailEngine.Domain.Abstractions;

public interface IEmailSender
{
    Task SendAsync(Email email);
}