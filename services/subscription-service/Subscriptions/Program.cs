using Subscriptions.Application.Internal.CommandServices;
using Subscriptions.Application.Internal.QueryServices;
using Subscriptions.Domain.Repositories;
using Subscriptions.Domain.Services;
using Subscriptions.Infrastructure.Persistence.EFC.Repositories;
using Subscriptions.Interfaces;
using Subscriptions.Interfaces.ACL;
using Subscriptions.Shared.Domain.Repositories;
using Subscriptions.Shared.Infrastructure.Persistence.EFC.Configuration;
using Subscriptions.Shared.Infrastructure.Persistence.EFC.Repositories;
using Subscriptions.Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Subscriptions.Application.External;
using Subscriptions.Application.External.OutBoundServices;
using Subscriptions.Domain.Model.Commands;
using Subscriptions.Infrastructure.IAM;
using Subscriptions.Interfaces.ACL.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var developmentString = builder.Configuration.GetConnectionString("DevelopmentConnection");

// Configure Database Context and Logging Levels

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseMySql(developmentString, ServerVersion.AutoDetect(developmentString))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        else if (builder.Environment.IsProduction())
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        }
    });
// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "AlquilaFacil.API",
                Version = "v1",
                Description = "Alquila Facil API",
                TermsOfService = new Uri("https://alquila-facil.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "Alquila Facil",
                    Email = "contact@alquilaf.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
    });

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddScoped<ISubscriptionInfoExternalService,SubscriptionInfoExternalService>();


// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Subscriptions Bounded Context Injection Configuration
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryServices, SubscriptionQueryService>();
builder.Services.AddScoped<ISubscriptionStatusRepository, SubscriptionStatusRepository>();
builder.Services.AddScoped<ISubscriptionStatusCommandService, SubscriptionStatusCommandService>();
builder.Services.AddScoped<ISubscriptionContextFacade, SubscriptionContextFacade>();
builder.Services.AddScoped<IExternalUserWithSubscriptionService, ExternalUserWithSubscriptionService>();

builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();


builder.Services.AddHttpClient();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8016); // ← importante
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    
    var planCommandService = services.GetRequiredService<IPlanCommandService>();
    await planCommandService.Handle(new CreatePlanCommand("Plan Premium", "El plan premium te permitira acceder a funcionalidades adicionales en la aplicacion", 20));
    
    var subscriptionStatusCommandService = services.GetRequiredService<ISubscriptionStatusCommandService>();
    await subscriptionStatusCommandService.Handle(new SeedSubscriptionStatusCommand());
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();