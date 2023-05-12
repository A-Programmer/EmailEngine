using Sadin.EmailEngine.Api;
using Sadin.EmailEngine.Application;
using Sadin.EmailEngine.Infrastructure;
using Sadin.EmailEngine.Domain;
using Sadin.EmailEngine.Persistence;
using Sadin.EmailEngine.Presentation;
using Sadin.EmailEngine.Shared;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration;

Configuration = builder.Environment.IsProduction()
    ? new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
    : new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

builder.Services.AddApiServices();
builder.Services.AddPresentationServices();
builder.Services.AddDomainServices();
builder.Services.AddApplicationServices(Configuration);
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices(Configuration);
builder.Services.AddSharedServices();

var app = builder.Build();

app.UseApi();
app.UsePresentation();
app.UseDomain();
app.UseApplication();
app.UsePersistence();
app.UseInfrastructure();
app.UseShared();
app.Run();
