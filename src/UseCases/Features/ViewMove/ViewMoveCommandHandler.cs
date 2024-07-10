namespace UseCases.Features;

public record ViewMoveCommand(ContainerRequest requestDto) : IRequest<Response<DriverContainerModel>>{}
public class ViewMoveCommandHandler : IRequestHandler<ViewMoveCommand, Response<DriverContainerModel>>
{
    private readonly IContainerRepository containerRepo;
    private readonly IUnitOfWork unitOfWork;

    private readonly IMoveStatusService moveStatusService;
    private readonly Result<SessionUserDto> sessionUser;

    public ViewMoveCommandHandler(
        IContainerRepository containerRepo, 
        IUnitOfWork unitOfWork, 
        IMoveStatusService moveStatusService, 
        IBaseService baseService
    )
    {
        this.containerRepo = containerRepo;
        this.unitOfWork = unitOfWork;
 
        this.moveStatusService = moveStatusService;
        this.sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<DriverContainerModel>> Handle(ViewMoveCommand request, CancellationToken cancellationToken)
    {     
        ContainerRequestDto? driverMove = new ContainerRequestDto(
            driverID: sessionUser.Value.driverID,
            proNumber: request.requestDto.ProNumber.GetProNumber(),
            legType: request.requestDto.LegType.GetLegType()
        );

        // Initialize new ViewMove
        DriverMoves viewMove = new DriverMoves() { 
            ProNumber = driverMove.proNumber.Value.Value, 
            DriverID = sessionUser.Value.driverID.Value.Value,
            CompanyID = sessionUser.Value.companyID.Value.Value.ToString(),
            LegType = driverMove.legType.Value.Value, 
            ViewMovedOn = DateTime.Now, 
            CreatedOn = DateTime.Now 
        };

        if (viewMove == null) return Response<DriverContainerModel>.Error(DriverMovesError<DriverMoves>.DriverMovesNotFound(driverMove.proNumber), driverMove);


        // Save Drive Move 
        await unitOfWork.DriverRepo.SaveDriverMove(viewMove);
        unitOfWork.Commit();

        // Return Preview Instruction
        var containerResult = await GetPreviewInstruction(driverMove);
        if (containerResult.IsFailure) return Response<DriverContainerModel>.Error(containerResult.Error);
        return Response<DriverContainerModel>.OK(containerResult.Value);
    }

    public async Task<Result<DriverContainerModel>> GetPreviewInstruction(ContainerRequestDto request)
    {
        var container = await containerRepo.GetContainer(new ContainerRequestDto(sessionUser.Value.driverID, request.proNumber, request.legType));
        if (container.IsFailure) return container.Error;

        var result = moveStatusService.GetStatus<DriverContainerModel>(container.Value);

        container.Value.MoveStatusID = result.Value.Value;
        return Result<DriverContainerModel>.Success(container.Value);
    }

    
}