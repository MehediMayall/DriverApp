namespace UseCases.Features;
public record GetPreviewInstructionQuery(ContainerRequestDto requestDto) : IRequest<Response<DriverContainerModel>>{}
public class GetPreviewInstructionQueryHandler: IRequestHandler<GetPreviewInstructionQuery, Response<DriverContainerModel>>
{
    private readonly IContainerRepository repo;
    private readonly IMoveStatusService moveStatusService;

    public GetPreviewInstructionQueryHandler(IContainerRepository repo, IMoveStatusService moveStatusService)
    {
        this.repo = repo;
        this.moveStatusService = moveStatusService;
    }
    public async Task<Response<DriverContainerModel>> Handle(GetPreviewInstructionQuery request, CancellationToken cancellationToken)
    {         
        var container = await repo.GetContainer(request.requestDto);
        if (container.IsFailure) return Response<DriverContainerModel>.Error(container.Error, request.requestDto);

        container.Value.MoveStatusID = moveStatusService.GetStatus<DriverContainerModel>(container.Value).Value.Value;
        return Response<DriverContainerModel>.OK(container.Value);
    }



}