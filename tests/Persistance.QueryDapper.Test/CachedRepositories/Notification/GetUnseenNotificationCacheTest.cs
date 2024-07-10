namespace Persistance.Query.Dapper.Test;

public class GetUnseenNotificationCacheTest : IClassFixture<ConfigurationFixture>
{
     private readonly DocumentNotificationRepositoryCached sut;
    private readonly DocumentNotificationRepository repo;
    private readonly ICacheService cacheService;


    public GetUnseenNotificationCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        repo = new DocumentNotificationRepository(fixture.configuration);
        sut = new DocumentNotificationRepositoryCached(repo, fixture.configuration, cacheService);
    }
 

    [Theory]
    [InlineData(765)]
    [InlineData(442)]
    public async Task GetUnseenNotification_ShouldReturn_List(int driverID)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverID);

        // Act
        var result = await sut.GetUnseenNotification(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.FirstOrDefault().DriverId.Should().Be(driverID);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task GetUnseenNotification_ShouldReturn_NotFound(int driverID)
    {
        // Arrange
        var driverid = new DriverID((NonNegative) driverID);

        // Act
        var result = await sut.GetUnseenNotification(driverid);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(NotificationError<IReadOnlyList<DocumentNotifications>>.UnseenNotificationNotFound(driverid));
    }


}