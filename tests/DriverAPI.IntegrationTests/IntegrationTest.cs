namespace DriverAPI.IntegrationTests;

[Trait("Category","Integration")]
public abstract class IntegrationTest: IClassFixture<TestApplication>
{
    protected readonly TestApplication testApplication;
    protected readonly HttpClient client;

    protected readonly ContainerMockData mockData;

    public IntegrationTest(TestApplication factory)
    {
        testApplication = factory;
        client = testApplication.CreateClient();
        mockData = new ContainerMockData();

        // Cofigure Authentication
        client.DefaultRequestHeaders.Authorization = mockData.GetToken();
    }

    protected async Task<T> Get<T>(string URL, HttpStatusCode ExpectedStatusCode = HttpStatusCode.OK)
    {
        var response = await client.GetAsync(URL);
        response.StatusCode.Should().Be(ExpectedStatusCode);
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<T> Post<T>(string URL, object RequestData, HttpStatusCode ExpectedStatusCode = HttpStatusCode.OK)
    {
        var response = await client.PostAsJsonAsync(URL, RequestData);
        response.StatusCode.Should().Be(ExpectedStatusCode);

        return await response.Content.ReadFromJsonAsync<T>();
    }

}