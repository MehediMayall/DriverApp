namespace UseCases.SharedServices;

public class WorkQueueService: IWorkQueueService
{
    private readonly IMoveStatusService moveStatusService;

    public WorkQueueService(IMoveStatusService moveStatusService)
    {
        this.moveStatusService = moveStatusService;
    }

    public async Task<List<WorkQueueModel>> Get(List<WorkQueueModel> workQueues)
    {
        List<WorkQueueModel> WorkQueues = workQueues.ToList();

        foreach (var item in WorkQueues)
        {
            var result = moveStatusService.GetStatus<WorkQueueModel>(item);
            if(result.IsSuccess) item.MoveStatusID = result.Value.Value;
        }
        
        List<WorkQueueModel> ProItems = new List<WorkQueueModel>();


        ProItems = WorkQueues.FindAll(p => p.IsRejected == 1);

        foreach (var item in ProItems)
        {
            WorkQueues.Find(p => p.Pro == item.Pro && p.IsRejected == 1).OrderNo = 9;
        }

        ProItems = WorkQueues.FindAll(p => p.IsRejected == null && p.DocumentID != null);

        foreach (var item in ProItems)
        {
            WorkQueues.Find(p => p.Pro == item.Pro && p.IsRejected == null && p.DocumentID != null).OrderNo = 9;
        }

        return  WorkQueues.OrderBy(p => p.OrderNo).ToList();
    }
}