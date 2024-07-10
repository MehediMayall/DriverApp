namespace DriverAPI.IntegrationTests;


public class UserControllerTests: IntegrationTest
{
    public UserControllerTests(TestApplication application):base(application){}
 

    [Fact]
    public async Task Index_ShouldReturn_APIVersion()
    {
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task HealthCheck()
    {
        var response = await client.GetAsync("/healthcheck");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AuthenticateUser_ShouldReturn_BadRequest()
    {
        var request = new LoginRequest("","","");
        var response = await client.PostAsJsonAsync("/api/driver/login", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AuthenticateUser_ShouldReturn_SuccessStatusCode()
    {
        var request = mockData.GetLoginRequest();
        var response = await client.PostAsJsonAsync("/api/driver/login", request);
        response.EnsureSuccessStatusCode();
    }
}