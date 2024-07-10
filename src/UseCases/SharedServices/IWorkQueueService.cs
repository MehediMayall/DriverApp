namespace UseCases.SharedServices;

public interface IWorkQueueService
{
    Task<List<WorkQueueModel>> Get(List<WorkQueueModel> WorkQueues);
}