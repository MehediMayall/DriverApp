namespace Persistance.Exceptions;

public static class ContainerRepositoryErrors<WorkQueueModel> where WorkQueueModel : class
{
    public static Error<WorkQueueModel> WorkQueueNotFound(DriverID driverID)
    {
        return new($"Couldn't find any workqueue data for driver: {driverID.Value}", "ContainerRepository.GetWorkQueue");
    }
  
}