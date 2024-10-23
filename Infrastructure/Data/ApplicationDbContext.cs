using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace dotnetcoreproject.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ContentPage> ContentPage { get; set; }
    public DbSet<Documents> Documents { get; set; }
    public DbSet<Lang> Lang { get; set; }
    public DbSet<LangDisplay> LangDisplay { get; set; }
    public DbSet<SiteConfig> SiteConfig { get; set; }
    public DbSet<Forms> Forms { get; set; }
    public DbSet<FormType> FormType { get; set; }
    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global query filter to exclude soft-deleted entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                if (method != null) method.Invoke(null, new object[] { modelBuilder });
            }
    }

    private static void SetGlobalQueryFilter<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => e.DeletedDate == null);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreaDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModDate = DateTime.UtcNow;
                    break;
            }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public static async Task EnsureDatabaseUpdatedAndSeeded(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            // Apply migrations
            await dbContext.Database.MigrateAsync();
            Log.Information("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during migration. Exception: {ExceptionDetail}", ex.ToString());
            throw;
        }

        try
        {
            // Seed data
            if (!dbContext.Lang.Any())
            {
                Log.Information("Seeding Lang table...");
                dbContext.Lang.AddRange(
                    new Lang
                    {
                        Name = "Türkçe",
                        Code = "TR",
                        IsDefault = true,
                        Logo = "Turkey.png"
                    },
                    new Lang { Name = "İngilizce", Code = "EN", IsDefault = false,Logo = "United Kingdom(Great Britain).png"}
                );
            }

            if (!dbContext.SiteConfig.Any(sc => sc.ConfigKey == "DataCacheDuration"))
            {
                Log.Information("Seeding SiteConfig table...");
                dbContext.SiteConfig.Add(new SiteConfig { ConfigKey = "DataCacheDuration", ConfigValue = "1440" });
            }

            if (!dbContext.SiteConfig.Any(sc => sc.ConfigKey == "ResponseCacheDuration"))
            {
                Log.Information("Seeding SiteConfig table...");
                dbContext.SiteConfig.Add(new SiteConfig { ConfigKey = "ResponseCacheDuration", ConfigValue = "1440" });
            }

            if (!dbContext.SiteConfig.Any(sc => sc.ConfigKey == "MetaDescription"))
            {
                Log.Information("Seeding SiteConfig table...");
                dbContext.SiteConfig.Add(new SiteConfig
                    { ConfigKey = "MetaDescription", ConfigValue = "MetaDescription" });
            }

            if (!dbContext.SiteConfig.Any(sc => sc.ConfigKey == "MetaKeywords"))
            {
                Log.Information("Seeding SiteConfig table...");
                dbContext.SiteConfig.Add(new SiteConfig { ConfigKey = "MetaKeywords", ConfigValue = "MetaKeywords" });
            }

            if (!dbContext.SiteConfig.Any(sc => sc.ConfigKey == "MetaTitle"))
            {
                Log.Information("Seeding SiteConfig table...");
                dbContext.SiteConfig.Add(new SiteConfig { ConfigKey = "MetaTitle", ConfigValue = "MetaTitle" });
            }


            if (!dbContext.FormType.Any())
            {
                Log.Information("Seeding FormType table...");
                dbContext.FormType.AddRange(
                    new FormType { Name = "Contact", FormCode = "Contact" },
                    new FormType { Name = "Register", FormCode = "Register" }
                );
            }

            if (!dbContext.ContentPage.Any())
            {
                Log.Information("Seeding User table...");
                dbContext.ContentPage.AddRange(new ContentPage
                {
                    Name = "MainPage", Link = "/", ContentTypes = ContentTypes.MainPage,
                    TemplateType = TemplateType.HtmlRaw
                },
                new ContentPage
                {
                    Name = "Footer",  ContentTypes = ContentTypes.FooterPage,
                    TemplateType = TemplateType.HtmlRaw
                },
                new ContentPage
                {
                    Name = "Menu1", Link = "menu1", ContentTypes = ContentTypes.GeneralPage,
                    TemplateType = TemplateType.HtmlRaw
                }
                    );
            }


            if (!dbContext.User.Any())
            {
                Log.Information("Seeding User table...");
                dbContext.User.AddRange(new User { Mail = "hybrid@gmail.com", Pass = "123", NameSurname = "Hybrid" });
            }

            await dbContext.SaveChangesAsync();
            Log.Information("Seed data applied successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during seeding. Exception: {ExceptionDetail}", ex.ToString());
            throw;
        }
    }
}