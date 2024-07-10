namespace DriverAPI.IntegrationTests;

public class BillOfLadingPDFTests: IntegrationTest
{

    private readonly string API;
    public BillOfLadingPDFTests(TestApplication application):base(application)
    {
        API = $"{UserMockData.BOL_PDF_API}{mockData.ProNumber_Delivery.Value}/DELIVERY";
    }

    [Fact]
    public async void GetBillOfLadingPDF_ShouldReturn_Authorization_Error()
    {
        // Arrange
        client.DefaultRequestHeaders.Authorization = null;
        
        // Act
        var response = await client.GetAsync(API);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // [Fact]
    // public async void GetBillOfLadingPDF_ShouldReturn_ValidPDFLink()
    // {
    //     // Arrange

    //     // Act
    //     var response = await httpClient.GetAsync( $"{UserMockData.BOL_PDF_API}{mockData.ProNumber_Delivery.Value}/DELIVERY");

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     var response = await response.Content.ReadFromJsonAsync<Response<DocumentResponseDto>>();

    //     response.Should().NotBeNull();
    //     response.Data.DocumentName.Length.Should().BeGreaterThan(10);
    // }
    


}