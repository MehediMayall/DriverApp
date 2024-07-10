namespace UseCases.Features;

public record GetUnseenNotificationQuery : IRequest<Response<IReadOnlyList<DocumentNotifications>>>{}
public class GetUnseenNotificationQueryHandler : IRequestHandler<GetUnseenNotificationQuery, Response<IReadOnlyList<DocumentNotifications>>>
{
    private readonly IDocumentNotificationRepository repo;
    private readonly Result<SessionUserDto> sessionUser;

    public GetUnseenNotificationQueryHandler(IDocumentNotificationRepository repo, IBaseService baseService)
    {
        this.repo = repo;
        this.sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<IReadOnlyList<DocumentNotifications>>> Handle(GetUnseenNotificationQuery request, CancellationToken cancellationToken)
    {
        var result =  await repo.GetUnseenNotification(sessionUser.Value.driverID);
        if (result.IsFailure) return Response<IReadOnlyList<DocumentNotifications>>.OK(null);  //return Response<IReadOnlyList<DocumentNotifications>>.Error(result.Error,sessionUser.Value);

         IReadOnlyList<DocumentNotifications> notifications = result.Value;
        notifications = notifications.OrderByDescending(d=> d.CreatedOn).ToList();

        return Response<IReadOnlyList<DocumentNotifications>>.OK(notifications);
    }
}