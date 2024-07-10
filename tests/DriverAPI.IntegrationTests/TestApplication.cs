using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DriverAPI.IntegrationTests;

public class TestApplication: WebApplicationFactory<Program>
{
    public IConfiguration configuration{ get; private set;}

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("test");

        // TEST CONFIGURATION
        builder.ConfigureAppConfiguration(config=>
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            config.AddConfiguration(configuration);
        });


        builder.ConfigureTestServices(services =>
        {
            services.Configure<JwtBearerOptions>(
	        JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.Configuration = new OpenIdConnectConfiguration
                    {
                        Issuer = JwtTokenProvider.Issuer,
                    };
                    // ValidIssuer and ValidAudience is not required, but it helps to define them as otherwise they can be overriden by for example the `user-jwts` tool which will cause the validation to fail
                    options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                    options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                    options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                }
            );

            // Fake Services
            services.AddSingleton<IImageService, FakeImageService>();
            services.AddSingleton<IEmailService, FakeEmailService>();
            services.AddSingleton<IPDFService, FakerPDFService>();

            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
        });
    }

     
}
