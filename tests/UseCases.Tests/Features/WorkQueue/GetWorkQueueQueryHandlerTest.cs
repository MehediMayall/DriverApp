namespace UseCases.Tests;

public class GetWorkQueueQueryHandlerTest
{
    private readonly IContainerRepository repo;
    private readonly IWorkQueueService workQueueService;
    private readonly IBaseService baseService;
    private readonly WorkQueueMockData workQueueMockData;
    
    public GetWorkQueueQueryHandlerTest()
    {
        repo = Substitute.For<IContainerRepository>();
        workQueueService = Substitute.For<IWorkQueueService>();
        baseService = Substitute.For<IBaseService>();

        workQueueMockData = new WorkQueueMockData();
    }


    [Fact]
    public async Task GetWorkQueueQueryHandler_ShouldReturn_Exception()
    {
        // Arrange
        var mockData = workQueueMockData.GetWorkQueueDataInString();
        WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(mockData);

        var workQueueList = new List<WorkQueueModel>(){ workQueue };

        repo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Failure(Error<List<WorkQueueModel>>.Set("No Data Found")));
        workQueueService.Get(workQueueList).Returns(workQueueList);
        baseService.GetSessionUser().Returns(workQueueMockData.GetSessionData());

        GetWorkQueueQuery workQueueQuery= new GetWorkQueueQuery();
        GetWorkQueueQueryHandler handler= new GetWorkQueueQueryHandler(repo, workQueueService, baseService);


        // Act
        var result = await handler.Handle(workQueueQuery, default);


        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeNull();
        result.Errors.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetWorkQueueQueryHandler_ShouldReturn_WorkQueue()
    {
        // Arrange
        var mockData = workQueueMockData.GetWorkQueueDataInString();
        WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(mockData);

        var workQueueList = new List<WorkQueueModel>(){ workQueue };

        repo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
        workQueueService.Get(Arg.Any<List<WorkQueueModel>>()).Returns(workQueueList);
        baseService.GetSessionUser().Returns(workQueueMockData.GetSessionData());

        GetWorkQueueQuery workQueueQuery= new GetWorkQueueQuery();
        GetWorkQueueQueryHandler handler= new GetWorkQueueQueryHandler(repo, workQueueService, baseService);


        // Act
        var result = await handler.Handle(workQueueQuery, default);


        // Assert
        result.Should().NotBeNull();
        result.Errors.Should().BeNull();
        result.Data.FirstOrDefault().Pro.Should().Be(workQueueList.FirstOrDefault().Pro);
    }
}
