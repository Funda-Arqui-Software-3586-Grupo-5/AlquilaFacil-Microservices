using LocalManagement.Application.External;
using LocalManagement.Application.External.OutboundServices;
using LocalManagement.Application.Internal.CommandServices;
using LocalManagement.Application.Internal.QueryServices;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Repositories;
using LocalManagement.Domain.Services;
using LocalManagement.Infrastructure.IAM;
using LocalManagement.Infrastructure.Persistence.EFC.Repositories;
using LocalManagement.Interfaces.ACL;
using LocalManagement.Interfaces.ACL.Services;
using LocalManagement.Shared.Domain.Repositories;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Repositories;
using LocalManagement.Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



builder.Services.AddScoped<ILocalCommandService, LocalCommandService>();
builder.Services.AddScoped<ILocalQueryService, LocalQueryService>();
builder.Services.AddScoped<ILocalsContextFacade, LocalsContextFacade>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();
builder.Services.AddScoped<ILocalCategoryRepository, LocalCategoryRepository>();

builder.Services.AddScoped<ILocalCategoryCommandService, LocalCategoryCommandService>();
builder.Services.AddScoped<ILocalCategoryQueryService, LocalCategoryQueryService>();

builder.Services.AddScoped<ICommentCommandService, CommentCommandService>();
builder.Services.AddScoped<ICommentQueryService, CommentQueryService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();


builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<IUserCommentExternalService, UserCommentExternalService>();
builder.Services.AddScoped<IUserExternalService, UserExternalService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8013); 
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    
    var localCategoryTypeCommandService = services.GetRequiredService<ILocalCategoryCommandService>();
    await localCategoryTypeCommandService.Handle(new SeedLocalCategoriesCommand());
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