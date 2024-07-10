using Persistance.Exceptions;

namespace Persistance.Query.Dapper.Test;

public class GetWorkQueueTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;

    public GetWorkQueueTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
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
    [InlineData(9999999)]
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