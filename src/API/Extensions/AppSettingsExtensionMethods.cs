namespace DriverApp.API;

public static class AppSettingsExtensionMethods
{
    public static T GetAppConfig<T>(this WebApplicationBuilder builder, string SectionName) 
    {
        return builder.Configuration.GetSection(SectionName).Get<T>() ??
            throw new ArgumentNullException($"{SectionName} is not found in AppSettings.", nameof(T));
    } 
    
}