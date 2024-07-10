using Serilog;

namespace UseCases;

[ExcludeFromCodeCoverage]
public static class UseCasesServiceRegistration
{
    public static IServiceCollection AddUseCasesService(this IServiceCollection services)
    {
        // AutoMapper
        try
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        }
        catch(Exception ex) {Log.Fatal(ex.GetAllExceptions());}


        services.AddScoped<IDirectoryService, DirectoryService>();
        services.AddScoped<IBaseService, BaseService>();
        services.AddScoped<IMoveStatusService, MoveStatusService>();
        services.AddScoped<IWorkQueueService, WorkQueueService>();
        services.AddScoped<IDeserializeService, DeserializeService>();

        services.AddScoped<IProofOfDeliveryService, ProofOfDeliveryService>();
        services.AddScoped<IProofOfDeliveryEmailService, ProofOfDeliveryEmailService>();
        
        services.AddScoped<IBeginMoveEmailService, BeginMoveEmailService>();
        

        return services;
    }
}