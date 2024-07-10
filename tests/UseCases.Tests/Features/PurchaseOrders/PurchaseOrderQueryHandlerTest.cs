namespace UseCases.Tests.Features.PurchaseOrders;

public class PurchaseOrderQueryHandlerTest
{
    private readonly IContainerRepository _containerRepo;
    private readonly IBaseService baseService;
    private readonly ContainerMockData mockData;

    public PurchaseOrderQueryHandlerTest()
    {
        _containerRepo = Substitute.For<IContainerRepository>();
        baseService = Substitute.For<IBaseService>();
        mockData = new ContainerMockData();
    }

    [Fact]
    public async Task GetPurchaseOrderQuery_Should_Return_ListOfPurchaseOrders()
    {
        // Arrange
        var mockReqDto = mockData.GetContainerRequestDto();
        var query = new GetPurchaseOrderQuery(mockReqDto);
        
        Result<IReadOnlyList<PurchaseOrder>> data = mockData.GetPurchaseOrders();

        baseService.GetSessionUser().Returns(mockData.GetSessionData());

        _containerRepo.GetPurchaseOrder(Arg.Any<CompanyID>(), query.requestDto.proNumber).Returns(data);
        var handler = new GetPurchaseOrderQueryHandler(_containerRepo, baseService);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Should().BeOfType<Result<IReadOnlyList<PurchaseOrder>>>();
        result.Value.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetPurchaseOrderQuery_Should_Return_Null()
    {
        // Arrange
        var mockReqDto = mockData.GetContainerRequestDto();
        var query = new GetPurchaseOrderQuery(mockReqDto);
        
        Result<IReadOnlyList<PurchaseOrder>> data = mockData.GetPurchaseOrdersErrorResult(mockReqDto.proNumber);

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        _containerRepo.GetPurchaseOrder(Arg.Any<CompanyID>(), query.requestDto.proNumber).Returns(data);
        var handler = new GetPurchaseOrderQueryHandler(_containerRepo, baseService);
        
        // Act
        var result = await handler.Handle(query, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Should().BeOfType<Result<IReadOnlyList<PurchaseOrder>>>();
        result.Value.Should().BeNull();
        result.Error.Message.Should().NotBeNullOrEmpty();
        result.Error.Message.Equals($"Couldn't find any purchase order data for ProNumber: {mockReqDto.proNumber.Value.Value}").Should().BeTrue();
    }
}
