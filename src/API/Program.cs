var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    // SERILOG
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    // JSON Format Config
    builder.Services.AddControllers()
        .AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.WriteIndented = false;
        });


    // Load AppSettings 
    builder.CacheAppSettings();

    // Caching
    builder.Services.AddDistributedMemoryCache();

    // Register Project Services
    builder.ConfigureProjectServices();

    var app = builder.Build();



    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    


    // GLOBAL EXCEPTION ERROR LOGGIN
    app.UseMiddleware<GlobalExceptionsLogger>();

    // REGISTER STATIC RESOURCE
    builder.RegisterStaticResource(app);

    // HEALTH CHECK
    app.MapHealthChecks("/healthcheck");


    app.MapControllers();
    app.UseAuthentication();
    app.UseAuthorization();
    Log.Information("App is started succesfully.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.GetAllExceptions());
}
finally{
    Log.CloseAndFlush();
}
 
// For Integration Tests
public partial class Program { }