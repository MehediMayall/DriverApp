namespace UseCases.Tests;

public class WorkQueueServiceTest
{
    private readonly IMoveStatusService moveStatusService;
    private readonly WorkQueueService sut;
    private readonly WorkQueueMockData mockData;

    public WorkQueueServiceTest()
    {
        moveStatusService = Substitute.For<IMoveStatusService>();
        sut = new WorkQueueService(moveStatusService);
        mockData = new WorkQueueMockData();
    }

    [Fact]
    public async Task WorkQueueService_ShouldReturn_ValidWorkQueue()
    {
        // Arrange 
        IReadOnlyList<WorkQueueModel> result;
        List<WorkQueueModel> workQueues = mockData.GetWorkQueues();
        var workQueue = workQueues.FirstOrDefault();

        moveStatusService.GetStatus(workQueue).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));

        List<WorkQueueModel> data = new List<WorkQueueModel>();
        data.Add(workQueue);


        // Act
        var ex = await Record.ExceptionAsync(async ()=>{
            result = await sut.Get(data);
        });

        // Assert
        ex.Should().BeNull();
    }

    [Fact]
    public async Task WorkQueueService_ShouldReturn_ValidWorkQueueWithRejected()
    {
        // Arrange 
        IReadOnlyList<WorkQueueModel> result;
        List<WorkQueueModel> workQueues = mockData.GetWorkQueues();
        workQueues[0].IsRejected = 1;
        
        var workQueue = workQueues.Where(c=> c.IsRejected == 1).FirstOrDefault();

        moveStatusService.GetStatus(workQueue).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));

        List<WorkQueueModel> data = new List<WorkQueueModel>();
        data.Add(workQueue);


        // Act
        var ex = await Record.ExceptionAsync(async ()=>{
            result = await sut.Get(data);
        });

        // Assert
        ex.Should().BeNull();
    }

    [Fact]
    public async Task WorkQueueService_ShouldReturn_ValidWorkQueueWithRejectedAndDocumentID()
    {
        // Arrange 
        IReadOnlyList<WorkQueueModel> result;
        List<WorkQueueModel> workQueues = mockData.GetWorkQueues();
        var rwq = workQueues[0];
        rwq.DocumentID = 10;
        var workQueue = workQueues.Where(c=> c.IsRejected is null && c.DocumentID is not null).FirstOrDefault();

        moveStatusService.GetStatus(workQueue).Returns(Result<MoveStatusID>.Success(new MoveStatusID((NonNegative) 1)));

        List<WorkQueueModel> data = new List<WorkQueueModel>();
        data.Add(workQueue);


        // Act
        var ex = await Record.ExceptionAsync(async ()=>{
            result = await sut.Get(data);
        });

        // Assert
        ex.Should().BeNull();
    }


}