using LocalManagement.Domain.Model.Aggregates;
using LocalManagement.Domain.Model.Entities;
using LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using Local = LocalManagement.Domain.Model.Aggregates.Local;

namespace LocalManagement.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        // Enable Audit Fields Interceptors
        builder.AddInterceptors();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        

        builder.Entity<LocalCategory>().HasKey(c => c.Id);
        builder.Entity<LocalCategory>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<LocalCategory>().Property(c => c.Name).IsRequired().HasMaxLength(30);
        builder.Entity<LocalCategory>().Property(c => c.PhotoUrl).IsRequired();


        builder.Entity<LocalCategory>().HasMany<Local>()
            .WithOne()
            .HasForeignKey(t => t.LocalCategoryId);
        
        
        
        
        builder.Entity<Local>().HasKey(p => p.Id);
        builder.Entity<Local>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Local>().Property(p => p.Features).IsRequired();
        builder.Entity<Local>().Property(p => p.Capacity).IsRequired();
        builder.Entity<Local>().Property(p => p.UserId).IsRequired();
        builder.Entity<Local>().OwnsOne(p => p.Price,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(p => p.PriceNight).HasColumnName("PriceNight");
            });
        builder.Entity<Local>().OwnsOne(p => p.LName,
            e =>
            {
                e.WithOwner().HasForeignKey("Id");
                e.Property(a => a.TypeLocal).HasColumnName("TypeLocal");
            });
        builder.Entity<Local>().OwnsOne(p => p.Address,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(s => s.District).HasColumnName("District");
                a.Property(s => s.Street).HasColumnName("Street");

            });
        builder.Entity<Local>().OwnsOne(p => p.Photo,
            h =>
            {
                h.WithOwner().HasForeignKey("Id");
                h.Property(g => g.PhotoUrlLink).HasColumnName("PhotoUrlLink");

            });
        builder.Entity<Local>().OwnsOne(p => p.Description,
            h =>
            {
                h.WithOwner().HasForeignKey("Id");
                h.Property(g => g.MessageDescription).HasColumnName("Description");

            });
        builder.Entity<Local>().OwnsOne(p => p.Place,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(s => s.Country).HasColumnName("Country");
                a.Property(s => s.City).HasColumnName("City");

            });
        
        builder.Entity<Comment>().HasKey(c => c.Id);
        builder.Entity<Comment>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Entity<Comment>().OwnsOne(c => c.Text,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(g => g.Text).HasColumnName("Text");
            });
        
        builder.Entity<Comment>().OwnsOne(c => c.Rating,
            n =>
            {
                n.WithOwner().HasForeignKey("Id");
                n.Property(g => g.Rating).HasColumnName("Rating");
            });
        
        builder.Entity<Comment>().HasOne<Local>().WithMany().HasForeignKey(l => l.LocalId);

        builder.Entity<Report>().HasKey(report => report.Id);
        builder.Entity<Report>().Property(report => report.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Report>().Property(report => report.Description).IsRequired();
        builder.Entity<Report>().Property(report => report.Title).IsRequired();
        builder.Entity<Report>().Property(report => report.CreatedAt).IsRequired();
        builder.Entity<Report>().Property(report => report.UserId).IsRequired();
        builder.Entity<Report>().HasOne<Local>().WithMany().HasForeignKey(r => r.LocalId);
        
        
            
        /*builder.Entity<User>()
            .HasMany(c => c.Profiles)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .HasPrincipalKey(t => t.Id);*/
            
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        

    }
}
