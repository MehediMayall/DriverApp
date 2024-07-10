namespace UseCases.Features;


public record GetWorkQueueQuery: IRequest<Response<List<WorkQueueModel>>>{}
public class GetWorkQueueQueryHandler: IRequestHandler<GetWorkQueueQuery, Response<List<WorkQueueModel>>>
{
    private readonly IContainerRepository repo;
    private readonly IWorkQueueService workQueueService;
    private readonly Result<SessionUserDto> sessionUser;

    public GetWorkQueueQueryHandler(IContainerRepository repo, IWorkQueueService workQueueService, IBaseService baseService)
    {
        this.repo = repo;
        this.workQueueService = workQueueService;
        this.sessionUser = baseService.GetSessionUser() ?? throw new UnauthorizedAccessException(ERRORS.SESSION_USER_NOT_FOUND);
    }

    public async Task<Response<List<WorkQueueModel>>> Handle(GetWorkQueueQuery request, CancellationToken cancellationToken)
    {
        var result = await repo.GetWorkQueue(this.sessionUser.Value.driverID);
        if (result.IsFailure) return Response<List<WorkQueueModel>>.Error(result.Error); //return Response<List<WorkQueueModel>>.Error(result.Error, sessionUser.Value);

        var WorkQueues = await workQueueService.Get(result.Value.ToList());

        return Response<List<WorkQueueModel>>.OK(WorkQueues);
    }

}