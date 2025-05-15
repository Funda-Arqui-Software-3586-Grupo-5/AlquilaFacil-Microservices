using IAM.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using IAM.Domain.Model.Aggregates;
using IAM.Domain.Model.Entities;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace IAM.Shared.Infrastructure.Persistence.EFC.Configuration;

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
        
        //IAM Context
        
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Username).IsRequired();
        builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired();
        builder.Entity<UserRole>().HasMany<User>().WithOne().HasForeignKey(u => u.RoleId);

        builder.Entity<UserRole>().HasKey(ur => ur.Id);
        builder.Entity<UserRole>().Property(ur => ur.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<UserRole>().Property(ur => ur.Role).IsRequired();


        //builder.Entity<User>().HasMany<Reservation>().WithOne().HasForeignKey(r => r.UserId);

        //builder.Entity<User>().HasMany<Notification>().WithOne().HasForeignKey(n => n.UserId);
        
    
        //builder.Entity<Comment>().HasOne<User>().WithMany().HasForeignKey(u => u.UserId);


        //builder.Entity<Report>().HasOne<User>().WithMany().HasForeignKey(r => r.UserId);
        
        /*builder.Entity<User>()
            .HasMany(c => c.Profiles)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .HasPrincipalKey(t => t.Id);*/
            
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        

    }
}
