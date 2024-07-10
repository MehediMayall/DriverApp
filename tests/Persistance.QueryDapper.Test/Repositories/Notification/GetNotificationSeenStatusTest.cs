namespace Persistance.Query.Dapper.Test;

public class GetNotificationSeenStatusTest : IClassFixture<ConfigurationFixture>
{
    private readonly DocumentNotificationRepository sut;
   
    public GetNotificationSeenStatusTest(ConfigurationFixture fixture)
    {
        sut = new DocumentNotificationRepository(fixture.configuration);
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
    [InlineData(-1)]
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