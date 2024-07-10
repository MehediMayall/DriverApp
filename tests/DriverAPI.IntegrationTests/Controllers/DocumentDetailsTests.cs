namespace DriverAPI.IntegrationTests;

public class DocumentDetailsTests: IntegrationTest
{

    private readonly string API;
    public DocumentDetailsTests(TestApplication application): base(application)
    {
        API = $"{UserMockData.DOCUMENT_DETAILS_API}{mockData.ProNumber.Value.Value}";
    }

    [Fact]
    public async void GetDocumentDetails_ShouldReturn_Authorization_Error()
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
    public async void GetDocumentDetails_ShouldReturn_ValidData(string LegType)
    {
        // Arrange
 
        // Act
        var response = await Get<Response<DocumentDetailAndPODto>>($"{API}/{LegType}");

        // Assert
        response.Data.Instructions.Pro.Should().Be(mockData.ProNumber.Value);
        response.Data.POList.Should().NotBeNull();
    }
    


}