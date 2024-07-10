namespace Persistance.Query.Dapper.Test;

public class GetNotificationSeenStatusCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly DocumentNotificationRepositoryCached sut;
    private readonly DocumentNotificationRepository repo;
    private readonly ICacheService cacheService;


    public GetNotificationSeenStatusCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        repo = new DocumentNotificationRepository(fixture.configuration);
        sut = new DocumentNotificationRepositoryCached(repo, fixture.configuration, cacheService);
    }
 

    [Theory]
    [InlineData(60)]
    [InlineData(70)]
    public async Task GetNotificationSeenStatus_ShouldReturn_List(int NotificationID)
    {
        // Arrange
 

        // Act
        var result = await sut.GetNotificationSeenStatus(NotificationID);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.Id.Should().Be(NotificationID);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99999)]
    public async Task GetNotificationSeenStatus_ShouldReturn_NotFound(int NotificationID)
    {
        // Arrange


        // Act
        var result = await sut.GetNotificationSeenStatus(NotificationID);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(NotificationError<DocumentNotifications>.NotificationSeenStatusNotFound(NotificationID));
    }


}