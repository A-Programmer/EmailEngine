using Sadin.Cms.Integration.Events.ContactUs;
using Sadin.Common.Abstractions;
using Sadin.EmailEngine.Application.Builders.EmailBuilders;
using Sadin.EmailEngine.Domain;
using Sadin.EmailEngine.Domain.Abstractions;
using Sadin.EmailEngine.Domain.Aggregates.EmailAggregate;

namespace Sadin.EmailEngine.Application.Services.Cms.ContactUs.EventHandlers;

public class ContactMessageCreatedEventHandler : IIntegrationEventHandler<ContactMessageCreatedEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ContactMessageCreatedEmailBuilder _emailBuilder;

    public ContactMessageCreatedEventHandler(IEmailSender emailSender, ContactMessageCreatedEmailBuilder emailBuilder)
    {
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _emailBuilder = emailBuilder;
    }

    public async Task Handle(ContactMessageCreatedEvent @event)
    {
        // TODO : Implement
        Email email =
            _emailBuilder.CreateEmail(@event.FullName, @event.Email, "Sadin.DEV- Your message has been received.", "", null);
        _emailSender.Send(EmailProvider.Custom, email);
        Console.WriteLine("\n\n\n\n Start processing event ... \n\n\n\n");
        Console.WriteLine("\n\n\n\n\n {0} \n\n {1} \n\n\n\n", @event.FullName, @event.Email);
    }
}