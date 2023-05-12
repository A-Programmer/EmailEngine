using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sadin.EmailEngine.Application.Builders.EmailBuilders;
using Sadin.EmailEngine.Application.Messaging.Models;
using Sadin.EmailEngine.Application.Services.Cms.ContactUs.EventHandlers;
using Sadin.EmailEngine.Application.Services.Cms.ContactUs.EventListeners;
using Sadin.EmailEngine.Domain.Abstractions;

namespace Sadin.EmailEngine.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg=>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);
        services.AddScoped<ContactMessageCreatedEventHandler>();
        services.AddHostedService<ContactMessageCreatedEventListener>();
        
        services.Configure<MessagingOptions>(
            configuration.GetSection("Messaging:RabbitMQ"));

        services.AddScoped<ContactMessageCreatedEmailBuilder>();
        
        return services;
    }

    public static WebApplication UseApplication(this WebApplication app)
    {
        return app;
    }
}