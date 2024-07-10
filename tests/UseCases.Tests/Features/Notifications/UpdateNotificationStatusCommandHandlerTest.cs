

namespace UseCases.Tests;

public class UpdateNotificationStatusCommandHandlerTest
{
    private readonly ContainerMockData mockData;
    private readonly INotificationCommandRepository repoCommand;
    private readonly IDocumentNotificationRepository repo;
    private readonly IBaseService baseService;
    private readonly IDeserializeService deserializeService;

    private readonly IUnitOfWork unitOfWork;
    private UpdateNotificationStatusCommandHandler sut;

    public UpdateNotificationStatusCommandHandlerTest()
    {
        repoCommand = Substitute.For<INotificationCommandRepository>();
        repo = Substitute.For<IDocumentNotificationRepository>();
        baseService = Substitute.For<IBaseService>();
        deserializeService = Substitute.For<IDeserializeService>();
        unitOfWork = Substitute.For<IUnitOfWork>();
        mockData = new ContainerMockData();

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        

    }





    [Fact]
    public async Task UpdateNotificationStatusCommandHandler_ShouldReturn_ARGUMENT_OBJECT_NULL()
    {
        // Arrange
        var errorResponse = Result<RequestedNotificationDto>.Failure(Error<RequestedNotificationDto>.Set(ERRORS.ARGUMENT_OBJECT_NULL));
        deserializeService.Get<RequestedNotificationDto>(mockData.EMPTY_PARAMETER).Returns(errorResponse);
        sut = new UpdateNotificationStatusCommandHandler(unitOfWork, repo, deserializeService, baseService);

        UpdateNotificationStatusCommand command = new UpdateNotificationStatusCommand(mockData.GetEmptyCommonRequestDto());

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault().Details.Should().Be(ERRORS.ARGUMENT_OBJECT_NULL);

    }


    [Fact]
    public async Task UpdateNotificationStatusCommandHandler_ShouldReturn_FAILED_DESERIALIZE_OBJECT()
    {
        // Arrange
        var errorResponse = Result<RequestedNotificationDto>.Failure(Error<RequestedNotificationDto>.Set(ERRORS.FAILED_DESERIALIZE_OBJECT));
        deserializeService.Get<RequestedNotificationDto>("test").Returns(errorResponse);
        sut = new UpdateNotificationStatusCommandHandler(unitOfWork, repo, deserializeService, baseService);

        UpdateNotificationStatusCommand command = new UpdateNotificationStatusCommand(new CommonRequestDto(Parameters:"test"));

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Errors.Should().HaveCount(1);
        result.Errors.FirstOrDefault().Details.Should().Be(ERRORS.FAILED_DESERIALIZE_OBJECT);

    }


    [Fact]
    public async Task UpdateNotificationStatusCommandHandler_ShouldReturn_NotificationNotFound()
    {
        // Arrange
        var notifications = mockData.GetNotifications();
        var updateNotification = notifications.Value.FirstOrDefault();
        updateNotification.SeenOn = DateTime.Now;

        var requestedNotification = new CommonRequestDto(Parameters:"{\"NotificatonId\":1}");

        // Deserialize 
        deserializeService.Get<RequestedNotificationDto>(requestedNotification.Parameters).Returns(Result<RequestedNotificationDto>.Success(new RequestedNotificationDto(1)));        
        // Find and Update Notification
        var errorResponse = NotificationError<DocumentNotifications>.NotificationNotFound(updateNotification.Id);
        unitOfWork.NotificationRepo.GetNotification(updateNotification.Id).Returns(errorResponse);
  
        sut = new UpdateNotificationStatusCommandHandler(unitOfWork, repo, deserializeService, baseService);
        UpdateNotificationStatusCommand command = new UpdateNotificationStatusCommand(requestedNotification);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Errors.Should().NotBeNull();
        result.Errors.FirstOrDefault().Details.Should().Be(errorResponse.Message);
    }

    [Fact]
    public async Task UpdateNotificationStatusCommandHandler_ShouldReturn_EmptyObject()
    {
        // Arrange
        var notifications = mockData.GetNotifications();
        var updateNotification = notifications.Value.FirstOrDefault();
        updateNotification.SeenOn = DateTime.Now;

        var requestedNotification = new CommonRequestDto(Parameters:"{\"NotificatonId\":1}");

        // Deserialize 
        deserializeService.Get<RequestedNotificationDto>(requestedNotification.Parameters).Returns(Result<RequestedNotificationDto>.Success(new RequestedNotificationDto(1)));        
        // Find and Update Notification
        unitOfWork.NotificationRepo.GetNotification(updateNotification.Id).Returns(Result<DocumentNotifications>.Success(updateNotification));
        // Return Unseen Notifications
        var errorResponse = NotificationError<IReadOnlyList<DocumentNotifications>>.UnseenNotificationNotFound(mockData.driverID);
        repo.GetUnseenNotification(mockData.driverID).Returns(errorResponse);


        sut = new UpdateNotificationStatusCommandHandler(unitOfWork, repo, deserializeService, baseService);
        UpdateNotificationStatusCommand command = new UpdateNotificationStatusCommand(requestedNotification);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Errors.Should().BeNull();
        result.Data.Should().BeNullOrEmpty();
    }


    [Fact]
    public async Task UpdateNotificationStatusCommandHandler_ShouldReturn_Notifications()
    {
        // Arrange
        var notifications = mockData.GetNotifications();
        var updateNotification = notifications.Value.FirstOrDefault();
        updateNotification.SeenOn = DateTime.Now;

        var requestedNotification = new CommonRequestDto(Parameters:"{\"NotificatonId\":1}");

        // Deserialize 
        deserializeService.Get<RequestedNotificationDto>(requestedNotification.Parameters).Returns(Result<RequestedNotificationDto>.Success(new RequestedNotificationDto(1)));        
        // Find and Update Notification
        unitOfWork.NotificationRepo.GetNotification(updateNotification.Id).Returns(Result<DocumentNotifications>.Success(updateNotification));
        // Return Unseen Notifications
        repo.GetUnseenNotification(mockData.driverID).Returns(notifications);


        sut = new UpdateNotificationStatusCommandHandler(unitOfWork, repo, deserializeService, baseService);
        UpdateNotificationStatusCommand command = new UpdateNotificationStatusCommand(requestedNotification);

        // Act
        var result = await sut.Handle(command, default);

        // Assert
        result.Errors.Should().BeNull();
        result.Data.Should().HaveCount(2);
    }
}