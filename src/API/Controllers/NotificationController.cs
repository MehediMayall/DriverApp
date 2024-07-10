
namespace  API.Controllers;


[ApiController]
[Route("api/[Controller]")]
public class NotificationController : ControllerBase
{
    private readonly IMediator mediator;

    public NotificationController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    // NOTIFICATION LIST
    [HttpGet("/api/driver/unseen/notification/list")]
    public async Task<Response<IReadOnlyList<DocumentNotifications>>> GetUnseenNotification()
    {
        return await mediator.Send(new GetUnseenNotificationQuery());
    }

    // NOTIFICATION LIST
    [HttpGet("/api/driver/recent/notification/list")]
    public async Task<Response<IReadOnlyList<DocumentNotifications>>> GetRecentNotification()
    {
        return await mediator.Send(new GetRecentNotificationQuery());
    }

    // UPDATE NOTIFICATION SEEN STATUS
    [HttpPost("/api/driver/notification/seen/status/update")]
    public async Task<Response<IReadOnlyList<DocumentNotifications>>> UpdateNotificationStatus([FromBody] CommonRequestDto request)
    {
        return await mediator.Send(new UpdateNotificationStatusCommand(request));
    }

} 