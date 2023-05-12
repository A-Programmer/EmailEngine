using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sadin.EmailEngine.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UsePersistence(this WebApplication app)
    {
        return app;
    }
}