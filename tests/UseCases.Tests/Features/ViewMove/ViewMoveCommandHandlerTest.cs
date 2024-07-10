// using NSubstitute.ReturnsExtensions;

// namespace UseCases.Tests;

// public class ViewMoveCommandHandlerTest
// {
//     private readonly IContainerRepository containerRepo;
//     private readonly IMoveStatusService moveStatusService;
//     private readonly IUnitOfWork unitOfWork;
//     private readonly IBaseService baseService;
//     private readonly IDeserializeService deserializeService;
//     private readonly ContainerMockData mockData;

//     public ViewMoveCommandHandlerTest()
//     {
//         unitOfWork = Substitute.For<IUnitOfWork>();
//         containerRepo = Substitute.For<IContainerRepository>();
//         moveStatusService = Substitute.For<IMoveStatusService>();
//         baseService = Substitute.For<IBaseService>();
//         deserializeService = Substitute.For<IDeserializeService>();

//         mockData = new ContainerMockData();
//     }


    

//     [Fact]
//     public async Task ViewMoveCommandHandler_ShouldReturn_NoContainerException()
//     {
//         // Arrange
//         ContainerRequestOldDto containerOldDto = mockData.GetContainerRequestOldDto();
//         ContainerRequestDto ContainerRequestDto = new ContainerRequestDto(
//             driverID : mockData.driverID, 
//             LegType : containerOldDto.LegType.GetLegType(), 
//             ProNumber : containerOldDto.ProNumber.GetProNumber()
//         );

//         DriverContainerModel driverContainer = mockData.GetPickupMTContainerModelDto();


//         baseService.GetSessionUser().Returns(mockData.GetSessionData());

//         containerRepo.GetContainer(ContainerRequestDto).Returns(Result<DriverContainerModel>.Failure(Error<DriverContainerModel>.Set("No container found")));
//         moveStatusService.GetStatus<DriverContainerModel>( driverContainer ).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));

//         ViewMoveCommand moveCommand =  new ViewMoveCommand(containerOldDto);
//         ViewMoveCommandHandler handler = new ViewMoveCommandHandler(containerRepo, unitOfWork, moveStatusService, baseService, deserializeService);

//         // Act
//         var result = await handler.Handle(moveCommand, default);

//         // Assert
//         result.Should().NotBeNull();
//         result.Data.Should().BeNull();
//         result.Errors.Should().HaveCountGreaterThan(0);
//         // ex.GetAllExceptions().Should().Be(ERRORS.FAILED_DESERIALIZE_OBJECT);
//     }


//     [Fact]
//     public async Task ViewMoveCommandHandler_ShouldReturn_PreviewInstruction()
//     {
//         // Arrange
//         ContainerRequestOldDto containerOldDto = mockData.GetContainerRequestOldDto();
//         ContainerRequestDto ContainerRequestDto = new ContainerRequestDto(
//             driverID : mockData.driverID, 
//             LegType : containerOldDto.LegType.GetLegType(), 
//             ProNumber : containerOldDto.ProNumber.GetProNumber()
//         );

//         DriverContainerModel driverContainer = mockData.GetPickupMTContainerModelDto();

//         baseService.GetSessionUser().Returns(mockData.GetSessionData());

//         containerRepo.GetContainer(ContainerRequestDto).Returns(Result<DriverContainerModel>.Success(driverContainer));
//         moveStatusService.GetStatus<DriverContainerModel>( driverContainer ).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));

//         ViewMoveCommand moveCommand =  new ViewMoveCommand(containerOldDto);
//         ViewMoveCommandHandler handler = new ViewMoveCommandHandler(containerRepo, unitOfWork, moveStatusService, baseService, deserializeService);

//         // Act
//         var result = await handler.Handle(moveCommand, default);

//         // Assert
//         result.Should().NotBeNull();
//         result.Data.Should().NotBeNull();

//         // ex.GetAllExceptions().Should().Be(ERRORS.FAILED_DESERIALIZE_OBJECT);
//     }
// }