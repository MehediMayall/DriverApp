namespace Persistance.Query.Dapper.Test;

public class GetContainerCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository repo;
    private readonly ContainerMockData mockData;

    private readonly ContainerRepositoryCached sut;
    private readonly ICacheService cacheService;

    public GetContainerCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        repo = new ContainerRepository(fixture.configuration);
        sut = new ContainerRepositoryCached(fixture.configuration, repo, cacheService);
        
        mockData = new ContainerMockData();
    }

    [Theory]
    [InlineData(27047, LegTypes.DELIVERY)]
    [InlineData(25104, LegTypes.PICKUP)]
    [InlineData(27047, LegTypes.PICKUP_MT)]
    public async Task GetContainer_ShouldReturn_List(int ProNumber, string LegType)
    {
        // Arrange
        var driverid = mockData.driverID;
  
        ContainerRequestDto requestDto= new ContainerRequestDto(driverid,ProNumber.GetProNumber() , LegType.GetLegType());

        // Act
        var result = await sut.GetContainer(requestDto);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.Pro.Should().Be(ProNumber);
        result.Value.LegType.ToUpper().Should().Be(LegType);
    }


    [Theory]
    [InlineData(1, 3, LegTypes.PICKUP_MT)]
    public async Task GetContainer_ShouldReturn_NoContainerResult(int driverId, int ProNumber, string LegType)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverId);
    
        ContainerRequestDto requestDto= new ContainerRequestDto(driverid,ProNumber.GetProNumber() , LegType.GetLegType());

        // Act
        var result = await sut.GetContainer(requestDto);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(PreviewInstructionErrors<DriverContainerModel>.NoContainerFound());
    }

}