using System.Data;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Application.Services;
using dotnetcoreproject.Infrastructure.Data;
using dotnetcoreproject.Infrastructure.Extensions;
using dotnetcoreproject.Infrastructure.Repositories;
using Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // IDbConnection'ı SqlConnection olarak kaydedin
        services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

        // IHttpContextAccessor'ı ekleyin
        services.AddHttpContextAccessor();

        // Repository'leri ve diğer servisleri kaydedin
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
        services.AddScoped<ISiteConfigRepository, SiteConfigRepository>();
        services.AddScoped<IContentPageRepository, ContentPageRepository>();
        services.AddScoped<IContentPageImageRepository, ContentPageImageRepository>();
        services.AddScoped<ILangRepository, LangRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILangRepository, LangRepository>();
        services.AddScoped<ILangDisplayRepository, LangDisplayRepository>();
        services.AddScoped<IDocumentsRepository, DocumentsRepository>();
        services.AddScoped<IFormTypeRepository, FormTypeRepository>();
        services.AddScoped<IFormsRepository, FormsRepository>();

        // IPropertyValueExtractor kaydedin
        services.AddScoped<IPropertyValueExtractor, PropertyValueExtractor>();


        return services;
    }

    public static IServiceCollection AddEntityFrameworkServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IBaseRepository<>), typeof(EFRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();

        return services;
    }
}