namespace UseCases.Features;
public record UpdateNotificationStatusCommand(CommonRequestDto requestDto) : IRequest<Response<IReadOnlyList<DocumentNotifications>>>{}
public class UpdateNotificationStatusCommandHandler : IRequestHandler<UpdateNotificationStatusCommand, Response<IReadOnlyList<DocumentNotifications>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IDocumentNotificationRepository queryRepo;
    private readonly IDeserializeService deserializeService;
    private readonly Result<SessionUserDto> sessionUser;

    public UpdateNotificationStatusCommandHandler(
        IUnitOfWork unitOfWork, 
        IDocumentNotificationRepository queryRepo, 
        IDeserializeService deserializeService,
        IBaseService baseService)
    {
        this.unitOfWork = unitOfWork;
        this.queryRepo = queryRepo;
        this.deserializeService = deserializeService;
        this.sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<IReadOnlyList<DocumentNotifications>>> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
    {
        // Deserialize 
        var deserializeResult = deserializeService.Get<RequestedNotificationDto>(request.requestDto.Parameters);
        if(deserializeResult.IsFailure) return Response<IReadOnlyList<DocumentNotifications>>.Error(deserializeResult.Error, request.requestDto.Parameters);


        // Find and Update Notification
        var updateResult = await UpdateNotification(deserializeResult.Value);
        if (!string.IsNullOrEmpty(updateResult)) return Response<IReadOnlyList<DocumentNotifications>>.Error(updateResult, request.requestDto.Parameters);


        // Return Unseen Notifications
        var notificationResult = await queryRepo.GetUnseenNotification(sessionUser.Value.driverID);
        if (notificationResult.IsFailure) return Response<IReadOnlyList<DocumentNotifications>>.OK(null);

        return Response<IReadOnlyList<DocumentNotifications>>.OK(notificationResult.Value);
    }

    private async Task<string> UpdateNotification(RequestedNotificationDto notificationDto)
    {
        // Get specific notification
        var result = await unitOfWork.NotificationRepo.GetNotification(notificationDto.NotificationID);
        if (result.IsFailure) return result.Error.Message;
        
        // Update seen on of that notification
        DocumentNotifications notification = result.Value;
        notification.SeenOn = DateTime.Now;
        await unitOfWork.NotificationRepo.UpdateDocumentNotifications(notification);
        unitOfWork.Commit();

        return "";
    }
}