namespace DriverAPI.IntegrationTests;

public class DocumentSubmitImageTests : IntegrationTest
{
    private readonly string API;

    public DocumentSubmitImageTests(TestApplication application) : base(application)
    {
         API = UserMockData.DOCUMENT_SUBMIT_IMAGE_API;
    }

    [Fact]
    public async void GetDocumentSubmitImage_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.PostAsJsonAsync(API, mockData.GetDocumentSubmitRequestDto(mockData.ProNumber.Value, "PICKUP"));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    


}