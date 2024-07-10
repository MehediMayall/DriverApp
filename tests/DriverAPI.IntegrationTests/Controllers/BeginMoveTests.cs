namespace DriverAPI.IntegrationTests;

public class BeginMoveTests : IntegrationTest
{
    private readonly string API;

    public BeginMoveTests(TestApplication application): base(application)
    {
        API = $"{UserMockData.BEGIN_MOVE_API}";
    }

    [Fact]
    public async void GetBeginMove_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.PostAsJsonAsync(API, mockData.GetContainerRequestDto());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("PICKUP")]
    [InlineData("PICKUP MT")]
    [InlineData("DELIVERY")]
    [InlineData("RETURN MT")]  
    public async void GetBeginMove_ShouldReturn_ValidData(string LegType)
    {
        // Arrange
        var requestDto = mockData.GetContainerRequest(LegType);

        // Act
        var response = await Post<Response<DriverContainerModel>>(API, requestDto);

        // Assert
        response.Data.Pro.Should().Be(requestDto.ProNumber);
        response.Data.LegType.Should().Be(requestDto.LegType);
    }
    

}