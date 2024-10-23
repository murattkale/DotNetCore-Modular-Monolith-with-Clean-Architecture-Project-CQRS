using System.Reflection;
using dotnetcoreproject.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetcoreproject.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        RegisterAutoMapper(services);

        // Register MediatR
        RegisterMediatR(services);

        // Register other application services
        // services.AddTransient<IMyApplicationService, MyApplicationService>();

        return services;
    }

    private static void RegisterAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    private static void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}