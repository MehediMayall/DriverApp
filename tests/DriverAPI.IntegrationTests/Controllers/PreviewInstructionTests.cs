namespace DriverAPI.IntegrationTests;

public class PreviewInstructionTests: IntegrationTest
{
    private readonly string API;

    public PreviewInstructionTests(TestApplication application):base(application)
     {
        API = $"{UserMockData.INSTRUCTIONS_API}{mockData.ProNumber.Value.Value}";
     }


    [Fact]
    public async void GetPreviewInstruction_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.GetAsync($"{UserMockData.PREVIEW_INSTRUCTIONS_API}{mockData.ProNumber.Value}/PICKUP");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    


    [Theory]
    [InlineData("PICKUP")]
    [InlineData("PICKUP MT")]
    [InlineData("DELIVERY")]
    [InlineData("RETURN MT")]    
    public async void GetPreviewInstruction_ShouldReturn_ValidData(string LegType)
    {
        // Arrange 

        // Act
        var response = await Get<Response<DriverContainerModel>>($"{API}/{LegType}");

        // Assert
        response.Data.Pro.Should().Be(mockData.ProNumber.Value.Value);
    }
    


}