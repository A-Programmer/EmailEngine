using Sadin.EmailEngine.Api.Middlewares;

namespace Sadin.EmailEngine.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection
            .AddScoped<ExceptionHandlingMiddleware>()
            .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
        return serviceCollection;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        app.UseCustomExceptionHandling();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}