namespace DriverAPI.IntegrationTests;

public class DocumentSubmitTests: IntegrationTest
{
    private readonly string API;

    public DocumentSubmitTests(TestApplication application): base(application)
    {
        API = UserMockData.DOCUMENT_SUBMIT_API;
    }

    [Fact]
    public async void GetDocumentSubmit_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.PostAsJsonAsync(API, mockData.GetDocumentSubmitRequestDto(mockData.ProNumber.Value, "PICkUP"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("PICKUP")]
    [InlineData("PICKUP MT")]
    [InlineData("DELIVERY")]
    [InlineData("RETURN MT")]  
    public async void GetDocumentSubmit_ShouldReturn_WorkQueue(string LegType)
    {
        // Arrange
        var requestDto = mockData.GetDocumentSubmitRequest(mockData.ProNumber.Value.Value, LegType);

        // Act
        var response = await Post<Response<List<WorkQueueModel>>>(API, requestDto);

        // Assert
        response.Errors.Should().BeNull();
        response.Data.Should().HaveCountGreaterThan(0);     
    }

}