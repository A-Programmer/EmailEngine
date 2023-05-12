using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sadin.EmailEngine.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UseShared(this WebApplication app)
    {
        return app;
    }
}