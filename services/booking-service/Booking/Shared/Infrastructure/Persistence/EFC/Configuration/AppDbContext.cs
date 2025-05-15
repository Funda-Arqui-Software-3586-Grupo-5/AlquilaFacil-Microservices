using Booking.Domain.Model.Aggregates;
using Booking.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Booking.Shared.Infrastructure.Persistence.EFC.Configuration;

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


        builder.Entity<Reservation>().HasKey(r => r.Id);
        builder.Entity<Reservation>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Reservation>().Property(r => r.StartDate).IsRequired();
        builder.Entity<Reservation>().Property(r => r.EndDate).IsRequired();
        builder.Entity<Reservation>().Property(r => r.UserId).IsRequired();
        builder.Entity<Reservation>().Property(r => r.LocalId).IsRequired();
        
        /*
        builder.Entity<User>().HasMany<Reservation>().WithOne().HasForeignKey(r => r.UserId);
        builder.Entity<Local>().HasMany<Reservation>().WithOne().HasForeignKey(r => r.LocalId);
        */
            
        /*builder.Entity<User>()
            .HasMany(c => c.Profiles)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .HasPrincipalKey(t => t.Id);*/
            
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        

    }
}
