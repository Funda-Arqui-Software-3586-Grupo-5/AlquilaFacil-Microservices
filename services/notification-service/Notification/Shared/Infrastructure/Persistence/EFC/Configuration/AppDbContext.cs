using Notification.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Notification.Domain.Models.Aggregates;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using Notification.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace Notifications.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        // Enable Audit Fields Interceptors
        builder.AddCreatedUpdatedInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Place here your entities configuration
        
        builder.Entity<Notification.Domain.Models.Aggregates.Notification>().HasKey(n => n.Id);
        builder.Entity<Notification.Domain.Models.Aggregates.Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Notification.Domain.Models.Aggregates.Notification>().Property(n => n.Title).IsRequired();
        builder.Entity<Notification.Domain.Models.Aggregates.Notification>().Property(n => n.Description).IsRequired();
        builder.Entity<Notification.Domain.Models.Aggregates.Notification>().Property(n => n.UserId).IsRequired();
        
            
        /*builder.Entity<User>()
            .HasMany(c => c.Profiles)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .HasPrincipalKey(t => t.Id);*/
            
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        

    }
}
