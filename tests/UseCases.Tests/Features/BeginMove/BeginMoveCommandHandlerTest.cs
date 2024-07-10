namespace UseCases.Tests;

public class BeginMoveCommandHandlerTest
{
    private readonly IContainerRepository containerRepo;
    private readonly IMoveStatusService moveStatusService;
    private readonly IUnitOfWork unitOfWork;
    private readonly IBaseService baseService;
    private readonly IBeginMoveEmailService beginMoveEmailService;

    private readonly ContainerMockData mockData;

    public BeginMoveCommandHandlerTest()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        containerRepo = Substitute.For<IContainerRepository>();
        moveStatusService = Substitute.For<IMoveStatusService>();
        baseService = Substitute.For<IBaseService>();
        beginMoveEmailService = Substitute.For<IBeginMoveEmailService>();

        mockData = new ContainerMockData();
    }


    
    [Fact]
    public async Task BeginMoveCommandHandler_ShouldReturn_NoContainerException()
    {
        // Arrange
        var sessionUser = mockData.GetSessionData();
        
        ContainerRequestDto requestDto = mockData.GetContainerRequestDto();


        unitOfWork.DriverRepo.GetDriverMove(sessionUser.Value.companyID, requestDto)
            .Returns(mockData.GetDriverMove(mockData.ProNumber_Pickup, requestDto.legType));
            
        containerRepo.GetContainer(requestDto).Returns(Result<DriverContainerModel>.Failure(Error<DriverContainerModel>.Set("Container Not Found")));
        baseService.GetSessionUser().Returns(mockData.GetSessionData());

        BeginMoveCommand moveCommand =  new BeginMoveCommand(mockData.GetContainerRequest());
        BeginMoveCommandHandler handler = new BeginMoveCommandHandler(containerRepo, unitOfWork, baseService, moveStatusService, beginMoveEmailService);

        var result = await handler.Handle(moveCommand, default);

        result.Should().NotBeNull();
        result.Data.Should().BeNull();
        result.Errors.Should().HaveCountGreaterThan(0);
    }



    [Fact]
    public async Task BeginMoveCommandHandler_ShouldReturn_PreviewInstruction()
    {
        // Arrange
        var sessionUser = mockData.GetSessionData();
        ContainerRequestDto requestDto = mockData.GetContainerRequestDto();

        var driverContainer = mockData.GetContainer(requestDto.legType);

        
        unitOfWork.DriverRepo.GetDriverMove( sessionUser.Value.companyID, requestDto)
            .Returns(mockData.GetDriverMove(mockData.ProNumber_Pickup, requestDto.legType));
            
        containerRepo.GetContainer(requestDto).Returns(driverContainer);
        baseService.GetSessionUser().Returns(mockData.GetSessionData());


        moveStatusService.GetStatus<DriverContainerModel>( driverContainer.Value ).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));


        BeginMoveCommand moveCommand =  new BeginMoveCommand(mockData.GetContainerRequest());
        BeginMoveCommandHandler handler = new BeginMoveCommandHandler(containerRepo, unitOfWork, baseService, moveStatusService, beginMoveEmailService);

        var result = await handler.Handle(moveCommand, default);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
    }
}