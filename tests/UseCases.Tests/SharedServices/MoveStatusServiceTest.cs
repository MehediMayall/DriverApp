namespace UseCases.Tests;

public class MoveStatusServiceTest
{

    private readonly MoveStatusService sut;
    private readonly ContainerMockData mockData;


    public MoveStatusServiceTest()
    {
        sut = new MoveStatusService();
        mockData = new ContainerMockData();
    }


    [Theory]
    [InlineData(null, null, 0)]
    [InlineData("2023-01-01", null, 1)]
    [InlineData("2023-01-01", "2023-01-01", 2)]
    [InlineData(null, "2023-01-01", 0)]
    public async Task MoveStatusService_ShouldReturn_ExpectedValue(string ViewMovedOn, string BeginMovedOn, int moveStatusID)
    {
        // Arrange
        DriverContainerModel container = mockData.GetPickupMTContainerModelDto();

        container.ViewMovedOn = ViewMovedOn;
        container.BeginMovedOn = BeginMovedOn;

        // Act
        Result<MoveStatusID> result = sut.GetStatus<DriverContainerModel>(container);

        // Assert

        result.Should().NotBeNull();
        result.Value.Should().Be(new MoveStatusID((NonNegative) moveStatusID));

    }
}