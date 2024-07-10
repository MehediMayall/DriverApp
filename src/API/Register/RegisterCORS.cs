namespace DriverApp.API;
public static class RegisterCORSExtensions
{
    public static void RegisterCORS(this WebApplicationBuilder builder)
    {
        try
        {
            var AllowedSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("Cors"));

            var CorsSettings = builder.Configuration.GetSection("Cors").Get<CorsSettings>();
            string origins = CorsSettings!.AllowedOrigin;
            if (string.IsNullOrEmpty(origins)) origins = "http://localhost;http://localhost:4200;";
            string[] allowedOrigins = origins.Split(";");

            bool AllowAllOrigin = false;

            foreach (string origin in allowedOrigins)
            {
                if (origin.Trim().CompareTo("*") == 0)
                {
                    AllowAllOrigin = true;
                    break;
                }
            }

            builder.Services.AddCors(options =>
            {
                if (AllowAllOrigin)
                {
                    options.AddPolicy(AllowedSpecificOrigins,
                        builder =>
                        {
                            builder.AllowAnyHeader()
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowCredentials();
                        });
                }
                else
                {
                    options.AddPolicy(AllowedSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins(allowedOrigins)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
                        });
                }
            });

            Log.Information($"RegisterCORS: SUCCECSSFULL.  AllowOrigin: {AllowAllOrigin}, Origin: {origins}");
        }
        catch (Exception ex){Log.Fatal(ex,"");}
    }
}
