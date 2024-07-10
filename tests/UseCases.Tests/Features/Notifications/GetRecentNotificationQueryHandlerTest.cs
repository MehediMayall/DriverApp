namespace UseCases.Tests;

public class GetRecentNotificationQueryHandlerTest
{
    private readonly ContainerMockData mockData;
    private readonly IDocumentNotificationRepository repo;
    private readonly IBaseService baseService;

    private readonly GetRecentNotificationQueryHandler sut;

    public GetRecentNotificationQueryHandlerTest()
    {
        mockData = new ContainerMockData();
        repo = Substitute.For<IDocumentNotificationRepository>();
        baseService = Substitute.For<IBaseService>();

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        sut = new GetRecentNotificationQueryHandler(repo,baseService);

    }


    [Fact]
    public async Task GetRecentNotificationQueryHandler_ShouldReturn_ErrorResponse()
    {
        // Arrange
        GetRecentNotificationQuery query = new GetRecentNotificationQuery();
        var errorResponse = NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(mockData.driverID);

        repo.GetNotifications(mockData.driverID).Returns(errorResponse);

        // Act
        var response = await sut.Handle(query, default);

        // Assert
        response.Data.Should().BeNull();
    }


    [Fact]
    public async Task GetRecentNotificationQueryHandler_ShouldReturn_OKResponse()
    {
        // Arrange
        GetRecentNotificationQuery query = new GetRecentNotificationQuery();
        var notifications = mockData.GetNotifications();
        repo.GetNotifications(mockData.driverID).Returns(notifications);

        // Act
        var response = await sut.Handle(query, default);

        // Assert
        response.Errors.Should().BeNull();
        response.Data.Should().NotBeNull();
        response.Data.Should().HaveCount(notifications.Value.Count);
    }


}