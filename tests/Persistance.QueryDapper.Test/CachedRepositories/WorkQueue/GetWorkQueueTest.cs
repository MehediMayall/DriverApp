using Persistance.Exceptions;


namespace Persistance.Query.Dapper.Test;

public class GetWorkQueueCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepositoryCached sut;
    private readonly ContainerRepository containerRepo;
    private readonly ContainerMockData mockData;

     private readonly ICacheService cacheService;

    public GetWorkQueueCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        containerRepo = new ContainerRepository(fixture.configuration);
        sut = new ContainerRepositoryCached(fixture.configuration, containerRepo, cacheService);
        mockData = new ContainerMockData();
    }

    [Fact]
    public async Task GetWorkQueue_ShouldReturn_List()
    {
        // Arrange
        var driverid = mockData.driverID;

        // Act
        var result = await sut.GetWorkQueue(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().HaveCountGreaterThan(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(999999)]
    public async Task GetWorkQueue_ShouldReturn_NotFound(int driverID)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverID);

        // Act
        var result = await sut.GetWorkQueue(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(ContainerRepositoryErrors<List<WorkQueueModel>>.WorkQueueNotFound(driverid));
            
    }
}