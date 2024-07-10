namespace Persistance.Query.Dapper.Test;

public class GetContainerTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;

    public GetContainerTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
        mockData = new ContainerMockData();
    }

    [Theory]
    [InlineData(25621, LegTypes.DELIVERY)]
    [InlineData(25621, LegTypes.PICKUP)]
    [InlineData(25621, LegTypes.PICKUP_MT)]
    public async Task GetContainer_ShouldReturn_List(int ProNumber, string LegType)
    {
        // Arrange
        var driverid = mockData.driverID;
        ContainerRequestDto requestDto= new ContainerRequestDto(driverid, ProNumber.GetProNumber() , LegType.GetLegType());

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
        var driverid = driverId.GetDriverID();
        ContainerRequestDto requestDto= new ContainerRequestDto(driverid, ProNumber.GetProNumber() , LegType.GetLegType());

        // Act
        var result = await sut.GetContainer(requestDto);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(PreviewInstructionErrors<DriverContainerModel>.NoContainerFound());
    }

}