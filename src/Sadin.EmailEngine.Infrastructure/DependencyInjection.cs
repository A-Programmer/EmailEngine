using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sadin.EmailEngine.Domain.Abstractions;
using Sadin.EmailEngine.Infrastructure.EmailSenders;
using Sadin.EmailEngine.Infrastructure.Models;

namespace Sadin.EmailEngine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CustomEmailProviderConfigurations>(
            configuration.GetSection("EmailProviders:CustomEmailProvider"));
        services.AddScoped<IEmailSender, CustomEmailSender>();
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        return app;
    }
}