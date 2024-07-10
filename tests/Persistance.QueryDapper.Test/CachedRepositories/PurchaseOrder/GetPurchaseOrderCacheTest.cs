namespace Persistance.Query.Dapper.Test;

public class GetPurchaseOrderCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository repo;
    private readonly ContainerMockData mockData;

    private readonly ContainerRepositoryCached sut;
    private readonly ICacheService cacheService;

    public GetPurchaseOrderCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        repo = new ContainerRepository(fixture.configuration);
        sut = new ContainerRepositoryCached(fixture.configuration, repo, cacheService);
        
        mockData = new ContainerMockData();
    }
 

    [Theory]
    [InlineData(25445)]
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