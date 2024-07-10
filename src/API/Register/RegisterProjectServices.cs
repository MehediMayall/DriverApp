using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Features;


namespace DriverApp.API;

public static class  RegisterProjectServices
{
    public static WebApplicationBuilder ConfigureProjectServices(this WebApplicationBuilder builder)
    {

         // OTHER 
        try
        {
            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 200; // 200 items max
                options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
            });
            builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        }
        catch(Exception ex) {Log.Fatal(ex.GetAllExceptions());}

        builder.Services.AddHttpContextAccessor();

        // MediatR
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));


        // Exception Log
        builder.Services.AddTransient<GlobalExceptionsLogger>();

        builder.Services.AddUseCasesService();
        builder.Services.AddPersistanceCommandServices(builder.Configuration);
        builder.Services.AddPersistanceQueryDapperServices(builder.Configuration);

        builder.Services.AddInfrastructureServices();

        // HEALTH CHECK
        builder.Services.AddHealthChecks();

        // JWT
        builder.RegisterJWT();
        
        // REGISTER CORS
        builder.RegisterCORS();

        

        return builder;
    }
}