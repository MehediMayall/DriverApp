using DriverApp.Infrastructure.Emails;
using Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Infrastructure;

public static class RegisterInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {

        services.AddSingleton<IImageService, ImageService>();
        services.AddSingleton<ICacheService, CacheService>();
        // services.AddSingleton<ICacheServiceRedis, CacheServiceRedis>();
        services.AddSingleton<IPDFService, PDFService>();
        services.AddSingleton<IFileOperationService, FileOperationService>();
        services.AddScoped<IHTMLReportService, HTMLReportService>();
        services.AddSingleton<IEmailService, EmailService>();
        return services;
    }
}