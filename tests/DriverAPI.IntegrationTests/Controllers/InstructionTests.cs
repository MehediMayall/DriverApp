namespace DriverAPI.IntegrationTests;

public class InstructionTests: IntegrationTest
{
    private readonly string API ;
    public InstructionTests(TestApplication application):base(application)
    {
        API = $"{UserMockData.INSTRUCTIONS_API}{mockData.ProNumber.Value.Value}";
    }
     
    [Fact]
    public async void GetInstruction_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.GetAsync($"{API}/PICKUP");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    

    [Theory]
    [InlineData("PICKUP")]
    [InlineData("PICKUP MT")]
    [InlineData("DELIVERY")]
    [InlineData("RETURN MT")]
    public async void GetInstruction_ShouldReturn_ValidData(string LegType)
    {
        // Arrange        

        // Act
        var response = await Get<Response<DriverContainerModel>>($"{API}/{LegType}");

        // Assert
        response.Data.Pro.Should().Be(mockData.ProNumber.Value.Value);        
    }


}