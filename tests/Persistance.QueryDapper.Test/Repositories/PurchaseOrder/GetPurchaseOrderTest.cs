namespace Persistance.Query.Dapper.Test;

public class GetPurchaseOrderTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;
   
    public GetPurchaseOrderTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
        mockData = new ContainerMockData();
   
    }
 

    [Theory]
    [InlineData(25445)]
    [InlineData(25121)]
    public async Task GetPurchaseOrder_ShouldReturn_List(int ProNumber)
    {
        // Arrange

        // Act
        var result = await sut.GetPurchaseOrder(mockData.companyID, ProNumber.GetProNumber());
 

        // Assert
        result.Should().NotBeNull();
        result.Value.Where(x=> x.ProNumber == ProNumber).Should().NotBeNull();
    }



    [Theory]
    [InlineData(0)]
    [InlineData(999999)]
    public async Task GetPurchaseOrder_ShouldReturn_NotFound(int ProNumber)
    {
        // Arrange

        // Act
        var result = await sut.GetPurchaseOrder(mockData.companyID, ProNumber.GetProNumber());
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(PurchaseOrderError<IReadOnlyList<PurchaseOrder>>.PurchaseOrderNotFound(ProNumber));
    }


    

}