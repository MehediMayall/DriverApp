namespace API.Controllers;


[ApiController]
[Authorize]
[Route("api/[Controller]")]
public class DriverController: BaseController
{
    private readonly IMediator mediator;


    public DriverController(IMediator mediator, IHttpContextAccessor httpContext):base(httpContext)
    {
        this.mediator = mediator;
    }

    
    // VIEW MOVE
    [HttpPost("/api/container/view/move/update")]
    public async Task<Response<DriverContainerModel>> SaveDriverViewMove([FromBody] ContainerRequest request)
    {
        // Log Activity
        LogDriverSubmitActivity($"VIEWED a pro move.", request);
        return await mediator.Send(new ViewMoveCommand(request));
    }
    
    // BEGIN MOVE
    [HttpPost("/api/container/begin/move/update")]
    public async Task<Response<DriverContainerModel>> SaveDriverBeginMove([FromBody] ContainerRequest request)
    {
        // Log Activity
        LogDriverSubmitActivity($"BEGUN a pro move.", request);
        return await mediator.Send(new BeginMoveCommand(request));
    }

    // DOCUMENT SUBMIT
    [HttpPost("/api/driver/document/submit")]
    public async Task<Response<List<WorkQueueModel>>> DocumentSubmit([FromBody] DocumentSubmitRequest request)
    {
        // Log Activity
        LogDriverSubmitActivity($"SUBMITTED a document. PRO# {request.ProNumber}, LegType: {request.LegType}", null);


        return request.LegType.ToUpper() switch 
        {
            LegTypes.PICKUP => await mediator.Send(new DocumentSubmitPickupCommand(request)),

            LegTypes.DELIVERY => await mediator.Send(new DocumentSubmitDeliveryCommand(request)),

            LegTypes.RETURN_MT => await mediator.Send(new DocumentSubmitPickupCommand(request)),

            LegTypes.PICKUP_MT => await mediator.Send(new DocumentSubmitPickupMTCommand(request)),

            _=> Response<List<WorkQueueModel>>.Error($"Document submit failed due to Invalid Leg Type: {request.LegType}")
        };
    }

    // DOCUMENT RE-SUBMIT
    // Document Resubmit is only for PICKUP and RETURN MT 
    [HttpPost("/api/driver/document/resubmit")]
    public async Task<Response<List<WorkQueueModel>>> DocumentReSubmit([FromBody] DocumentSubmitRequest requestDto)
    {
        // Log Activity
        LogDriverSubmitActivity($"RESUBMITTED a document. PRO# {requestDto.ProNumber}, LegType: {requestDto.LegType}", null);

        requestDto.IsResubmit = SubmitTypes.RE_SUBMIT;

        return requestDto.LegType.ToUpper() switch 
        {
            LegTypes.PICKUP => await mediator.Send(new DocumentSubmitPickupCommand(requestDto)),

            LegTypes.RETURN_MT => await mediator.Send(new DocumentSubmitPickupCommand(requestDto)),

            _=> Response<List<WorkQueueModel>>.Error($"Document submit failed due to Invalid Leg Type: {requestDto.LegType}")
        };
    }

    [HttpPost("/api/driver/document/submit/image/upload")]
    public async Task<Response<string>> documentSubmitImageUpload([FromBody] DocumentImageDto request)
    {
        // Log Activity
        LogDriverSubmitActivity($"SUBMITTED a document IMAGE.", null);
        return await this.mediator.Send(new DocumentImageUploadCommand(request));
    }

    protected void LogDriverSubmitActivity(string Message, object request)
    {
        try
        {           
            var user = GetSessionUser();             
            Log.Information($"{user.Division}-{user.driverID.Value.Value} [{user.DriverName}]: {Message}\t[{ request.GetJsonString<object>()}]");
        }
        catch(Exception ex){Log.Error("LogDriverSubmitActivity=> " + ex.GetAllExceptions());}
    }

}