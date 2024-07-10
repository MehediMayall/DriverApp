using Microsoft.Extensions.FileProviders;

namespace DriverApp.API;
public static class RegisterStaticResourceExtension
{
    public static void RegisterStaticResource(this WebApplicationBuilder builder, WebApplication app)
    {
        
        // ******************************* STATIC FILES AND ATTACHMENT FOLDER
        var StaticFiles = builder.GetAppConfig<StaticFileDto>("StaticFiles");
        string staticFileDirectory = StaticFiles.BasePath.Trim();
        var directoryNames = builder.GetAppConfig<AttachmentDirectoriesDto>("AttachmentDirectories");

        // DOCUMENTS
        if (Directory.Exists(staticFileDirectory))
            RegisterPath(app,"documents", staticFileDirectory);
        else 
            Log.Error($"DOCUMENT FOLDER NOT FOUND IN >> {staticFileDirectory}");


        // BILL OF LADING
        string bolPath = Path.Combine(Directory.GetCurrentDirectory(), directoryNames.BOL);

        if (Directory.Exists(bolPath))
            RegisterPath(app, "bol", bolPath);
        else 
            Log.Error($"Generated_BOL FOLDER NOT FOUND IN >> {bolPath}");


        // ATTACHMENT
        string attachmentsPath = Path.Combine(Directory.GetCurrentDirectory(), directoryNames.ATTACHMENT);

        if (Directory.Exists(attachmentsPath))
            RegisterPath(app, "attachments", attachmentsPath);
        else 
            Log.Error($"ATTACHMENT FOLDER NOT FOUND IN >>{attachmentsPath}");


        Log.Information("RegisterStaticResource: SUCCECSSFULL");
        
    }

    private static void RegisterPath(WebApplication app, string RequestPath, string PhysicalPath)
    {
        try
        {
            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(PhysicalPath),
                RequestPath = new PathString($"/{RequestPath}")
            });

            Log.Information($"{RequestPath.ToUpper()}: SUCCECSSFULL");
        }
        catch (Exception ex) { Log.Fatal(ex, ""); }
    }
}
