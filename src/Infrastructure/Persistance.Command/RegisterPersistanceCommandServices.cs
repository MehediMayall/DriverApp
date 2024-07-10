using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Command;

public static class RegisterPersistanceCommandServices
{
    public static IServiceCollection AddPersistanceCommandServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Driver DB Context
        services.AddDbContext<DriverContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });
        // WebPortal DB Context
        services.AddDbContext<WebPortalContext>(options =>{
            options.UseSqlServer(configuration.GetConnectionString("WebPortal"));
        });

        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<INotificationCommandRepository, NotificationCommandRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUnitOfWork_WebPortal, UnitOfWork_WebPortal>();
        return services;
    }
}