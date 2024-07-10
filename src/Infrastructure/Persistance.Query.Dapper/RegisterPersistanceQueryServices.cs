using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance.Query.Dapper;

public static class RegisterPersistanceQueryDapperServices
{
    public static IServiceCollection AddPersistanceQueryDapperServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<UserRepository>();
        services.AddScoped<IUserRepository, UserRepositoryCached>();

        services.AddScoped<ContainerRepository>();
        services.AddScoped<IContainerRepository, ContainerRepositoryCached>();


        services.AddScoped<DocumentNotificationRepository>();
        services.AddScoped<IDocumentNotificationRepository, DocumentNotificationRepositoryCached>();
        return services;
    }
}