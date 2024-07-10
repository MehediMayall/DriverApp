namespace DriverApp.API;

public static class CacheAppSettingsExtensions
{
    public static void CacheAppSettings(this WebApplicationBuilder builder)
    {
        // Token Management
        builder.Services.Configure<TokenManagementDto>(builder.Configuration.GetSection("TokenManagement"));

        // Email Configuration
        builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

        // Web Portal URL
        builder.Services.Configure<WebPortalURLDto>(builder.Configuration.GetSection("WebPortalURL"));
        
        // Attachment Directories
        builder.Services.Configure<AttachmentDirectoriesDto>(builder.Configuration.GetSection("AttachmentDirectories"));
        
        // Static Files
        builder.Services.Configure<StaticFileDto>(builder.Configuration.GetSection("StaticFiles"));

        // Template Paths
        builder.Services.Configure<TemplatePathsDto>(builder.Configuration.GetSection("TemplatePaths"));
    }
}