namespace UseCases.Tests;

public class GetUnseenNotificationQueryHandlerTest
{
    private readonly ContainerMockData mockData;
    private readonly IDocumentNotificationRepository repo;
    private readonly IBaseService baseService;

    private readonly GetUnseenNotificationQueryHandler sut;

    public GetUnseenNotificationQueryHandlerTest()
    {
        mockData = new ContainerMockData();
        repo = Substitute.For<IDocumentNotificationRepository>();
        baseService = Substitute.For<IBaseService>();

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        sut = new GetUnseenNotificationQueryHandler(repo,baseService);
    }


    [Fact]
    public async Task GetUnseenNotificationQueryHandler_ShouldReturn_ErrorResponse()
    {
        // Arrange
        GetUnseenNotificationQuery query = new GetUnseenNotificationQuery();
        var errorResponse = NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(mockData.driverID);

        repo.GetUnseenNotification(mockData.driverID).Returns(errorResponse);

        // Act
        var response = await sut.Handle(query, default);

        // Assert
        response.Data.Should().BeNull();
    }


    [Fact]
    public async Task GetUnseenNotificationQueryHandler_ShouldReturn_OKResponse()
    {
        // Arrange
        GetUnseenNotificationQuery query = new GetUnseenNotificationQuery();
        var notifications = mockData.GetNotifications();
        repo.GetUnseenNotification(mockData.driverID).Returns(notifications);

        // Act
        var response = await sut.Handle(query, default);

        // Assert
        response.Errors.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(notifications.Value.Count);
    }


}