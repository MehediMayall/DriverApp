namespace DriverAPI.IntegrationTests;

public class DocumentDetailsPDFTests : IntegrationTest
{

    private readonly string API;
    public DocumentDetailsPDFTests(TestApplication application): base(application)
    {
        API = $"{UserMockData.DOCUMENT_DETAILS_PDF_API}{mockData.ProNumber.Value}/8";
    }

    [Fact]
    public async void GetDocumentDetailsPDF_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await client.GetAsync(API);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    


}