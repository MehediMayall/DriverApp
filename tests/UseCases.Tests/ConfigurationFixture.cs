using Microsoft.AspNetCore.TestHost;

namespace UseCases.Tests;

public class ConfigurationFixture : IDisposable
{
    private readonly TestServer server;
    private readonly HttpClient client;
    public IConfiguration configuration;

    public ConfigurationFixture()
    {
        server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        client = server.CreateClient();
        
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
 
        configuration = builder.Build();
      
    }

    public IServiceProvider ServiceProvider => server.Host.Services;

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            server.Dispose();
            client.Dispose();
        }
    }
}