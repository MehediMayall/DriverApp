

namespace UseCases.Tests;

public class GetPreviewInstructionQueryHandlerTest
{
    private readonly IContainerRepository containerRepo;
    private readonly IMoveStatusService moveStatusService;

    private readonly ContainerMockData mockData;

    public GetPreviewInstructionQueryHandlerTest()
    {
        this.containerRepo = Substitute.For<IContainerRepository>();
        this.moveStatusService = Substitute.For<IMoveStatusService>();
        mockData = new ContainerMockData();
    }

    [Fact]
    public  async Task GetPreviewInstructionQuery_ShouldReturn_DataObjectNull_Exception()
    {
        // Arrange
        ContainerRequestDto invalidContainerRequestDto = mockData.GetContainerRequestDtoWithInvalidData();
        DriverContainerModel nullWorkModel = null;
        containerRepo.GetContainer(invalidContainerRequestDto).Returns(Result<DriverContainerModel>.Success(nullWorkModel));

        GetPreviewInstructionQuery query = new GetPreviewInstructionQuery(invalidContainerRequestDto);
        GetPreviewInstructionQueryHandler handler = new GetPreviewInstructionQueryHandler(containerRepo, moveStatusService);

        // Act
        var ex = await Record.ExceptionAsync(async ()=>{
             await handler.Handle(query, default);
        });

        // Assert
        ex.Should().NotBeNull();
        // ex.GetAllExceptions().Should().Be(ERRORS.DATA_OBJECT_NULL);
    }

    [Fact]
    public  async Task GetPreviewInstructionQuery_ShouldNotReturn_DataObjectNull_Exception()
    {
        // Arrange
        ContainerRequestDto ContainerRequestDto = mockData.GetContainerRequestDto();
        DriverContainerModel driverContainer = mockData.GetPickupMTContainerModelDto();

        moveStatusService.GetStatus<DriverContainerModel>( driverContainer ).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));
        containerRepo.GetContainer(ContainerRequestDto).Returns(Result<DriverContainerModel>.Success(driverContainer));

        GetPreviewInstructionQuery query = new GetPreviewInstructionQuery(ContainerRequestDto);
        GetPreviewInstructionQueryHandler handler = new GetPreviewInstructionQueryHandler(containerRepo, moveStatusService);


        var ex = await Record.ExceptionAsync(async ()=>{
             await handler.Handle(query, default);
        });

        ex.Should().BeNull();
    }

    [Fact]
    public  async Task GetPreviewInstructionQuery_ShouldNotReturn_Data()
    {
        // Arrange
        ContainerRequestDto ContainerRequestDto = mockData.GetContainerRequestDto();
        DriverContainerModel driverContainer = mockData.GetPickupMTContainerModelDto();

        moveStatusService.GetStatus<DriverContainerModel>( driverContainer ).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));
        containerRepo.GetContainer(ContainerRequestDto).Returns(Result<DriverContainerModel>.Success(driverContainer));

        GetPreviewInstructionQuery query = new GetPreviewInstructionQuery(ContainerRequestDto);
        GetPreviewInstructionQueryHandler handler = new GetPreviewInstructionQueryHandler(containerRepo, moveStatusService);


        var result = await handler.Handle(query, default);
        result.Should().NotBeNull();
        result.Data.Pro.Should().Be(driverContainer.Pro);
    }
}