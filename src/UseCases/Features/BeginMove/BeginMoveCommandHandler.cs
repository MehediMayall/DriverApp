namespace UseCases.Features;

public record BeginMoveCommand(ContainerRequest requestDto) : IRequest<Response<DriverContainerModel>>{}

public class BeginMoveCommandHandler: IRequestHandler<BeginMoveCommand, Response<DriverContainerModel>>
{
    private readonly IContainerRepository containerRepo;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMoveStatusService moveStatusService;
    private readonly IBeginMoveEmailService beginMoveEmailService;
    private readonly Result<SessionUserDto> sessionUser;

    public BeginMoveCommandHandler(
        IContainerRepository containerRepo, 
        IUnitOfWork unitOfWork, 
        IBaseService baseService, 
        IMoveStatusService moveStatusService, 
        IBeginMoveEmailService beginMoveEmailService
        )
    {
        this.containerRepo = containerRepo;
        this.unitOfWork = unitOfWork;
        this.moveStatusService = moveStatusService;
        this.beginMoveEmailService = beginMoveEmailService;
        sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<DriverContainerModel>> Handle(BeginMoveCommand request, CancellationToken cancellationToken)
    {

        ContainerRequestDto? requestDto = new ContainerRequestDto(
            driverID: sessionUser.Value.driverID,
            proNumber: request.requestDto.ProNumber.GetProNumber(),
            legType: request.requestDto.LegType.GetLegType()
        );

       // GET Last Viewed Move
        var driverMoveResult = await unitOfWork.DriverRepo.GetDriverMove( sessionUser.Value.companyID, requestDto); 
        if (driverMoveResult.IsFailure) 
            return Response<DriverContainerModel>.Error(driverMoveResult.Error, requestDto);

        // SET Updated move information
        DriverMoves driverMove = driverMoveResult.Value;
        driverMove.ProNumber = requestDto.proNumber.Value.Value;
        driverMove.DriverID = sessionUser.Value.driverID.Value.Value;
        driverMove.CompanyID = sessionUser.Value.companyID.Value.Value.ToString();
        driverMove.LegType = requestDto.legType.Value.Value;
        driverMove.BeginMovedOn = DateTime.Now;



        // Update Begin Move
        DriverMoves updatedMove = await this.unitOfWork.DriverRepo.UpdateDriverMove(driverMoveResult.Value);
        

        // RETURN PREVIEW INSTRUCTION
        var containerResult = await GetPreviewInstruction(requestDto);
        if (containerResult.IsFailure) 
            return Response<DriverContainerModel>.Error(containerResult.Error);

        DriverContainerModel container = containerResult.Value;

        try
        {
            // BEGIN MOVE EMAIL NOTIFICATION
            if (driverMove.LegType.ToUpper() != LegTypes.DELIVERY && (driverMove.IsEmailSent?? false) == false) 
            {
                var resultEmail = await beginMoveEmailService.SendBeginMoveEmail(container, driverMove);
                if (resultEmail.IsFailure) Log.Error(resultEmail.Error.Message);
            }

            // Update Email Sent Status
            updatedMove.IsEmailSent = true;
            await this.unitOfWork.DriverRepo.UpdateDriverMove(driverMoveResult.Value);

        }
        catch (Exception ex){ Log.Fatal(ex.GetAllExceptions()); }


        // COMMIT
        unitOfWork.Commit();
        return Response<DriverContainerModel>.OK(container);
    }

    public async Task<Result<DriverContainerModel>> GetPreviewInstruction(ContainerRequestDto request)
    {
        var container = await containerRepo.GetContainer(new ContainerRequestDto(sessionUser.Value.driverID, request.proNumber, request.legType));
        if (container.IsFailure) return container.Error;

        container.Value.MoveStatusID = moveStatusService.GetStatus(container.Value).Value.Value;
        return Result<DriverContainerModel>.Success(container.Value);
    }

  
}