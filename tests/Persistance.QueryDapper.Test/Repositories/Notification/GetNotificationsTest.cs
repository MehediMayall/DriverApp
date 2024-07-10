namespace Persistance.Query.Dapper.Test;

public class GetNotificationsTest : IClassFixture<ConfigurationFixture>
{
    private readonly DocumentNotificationRepository sut;
   
    public GetNotificationsTest(ConfigurationFixture fixture)
    {
        sut = new DocumentNotificationRepository(fixture.configuration);   
    }
 

    [Theory]
    [InlineData(765)]
    [InlineData(442)]
    public async Task GetNotifications_ShouldReturn_List(int driverID)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverID);

        // Act
        var result = await sut.GetNotifications(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.FirstOrDefault().DriverId.Should().Be(driverID);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task GetNotifications_ShouldReturn_NotFound(int driverID)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverID);

        // Act
        var result = await sut.GetNotifications(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(driverid));
    }


}