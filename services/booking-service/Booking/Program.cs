using Booking.Application.Internal.CommandServices;
using Booking.Application.External;
using Booking.Application.External.OutboundServices;
using Booking.Application.Internal.QueryServices;
using Booking.Domain.Repositories;
using Booking.Domain.Services;
using Booking.Infrastructure.Persistence.EFC.Repositories;
using Booking.Interfaces;
using Booking.Interfaces.ACL;
using Booking.Interfaces.ACL.Services;
using Booking.Shared.Domain.Repositories;
using Booking.Shared.Infrastructure.Persistence.EFC.Configuration;
using Booking.Shared.Infrastructure.Persistence.EFC.Repositories;
using Booking.Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Booking.Application.Internal.Consumers;
using Booking.Application.Internal.Publishers;
using Booking.Domain.AMQP;
using Booking.Shared.Application.EventHandlers;
using Booking.Shared.Application.Hosted;
using Booking.Shared.Domain.Model.ValueObjects;
using RabbitMQ.Client;
using System.Reflection;

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
        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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

builder.Services.AddScoped<IMessagePublisher, MessagePublisher>();


// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Booking Bounded Context Injection Configuration
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();

builder.Services.AddScoped<ILocalExternalService, LocalExternalService>();
builder.Services.AddScoped<ISubscriptionExternalService, SubscriptionExternalService>();
builder.Services.AddScoped<IUserReservationExternalService, UserReservationExternalService>();

builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<ILocalsContextFacade, LocalsContextFacade>();
builder.Services.AddScoped<ISubscriptionContextFacade, SubscriptionContextFacade>();

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8017); // ← importante
});

var factory = new ConnectionFactory
{
    Uri = new Uri(builder.Configuration["Host"]),
    DispatchConsumersAsync = true
};

var connection = factory.CreateConnection();
var channel = connection.CreateModel();

builder.Services.AddSingleton(connection);
builder.Services.AddSingleton(channel);
builder.Services.AddSingleton(new RabbitMqChannelWrapper(channel));
builder.Services.AddSingleton<MessageConsumer>();
builder.Services.AddHostedService<RabbitMqConsumerHostedService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    services.StartPublishing(builder.Configuration, channel);
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
