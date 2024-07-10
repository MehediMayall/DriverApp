using Domain.Shared;

namespace Infrastructure.Persistance.Command;

public class NotificationCommandRepository : GenericRepository<DocumentNotifications>, INotificationCommandRepository
{
    private readonly DriverContext dbContext;
    private readonly ICacheService cacheService;

    public NotificationCommandRepository(DriverContext dbContext, ICacheService cacheService):base(dbContext)
    {
        this.dbContext = dbContext;
        this.cacheService = cacheService;
    }

    public async Task<Result<DocumentNotifications>> GetNotification(int NotificationID)
    {
        var data = await new GenericRepository<DocumentNotifications>(dbContext).GetOneByID(NotificationID);
        if (data is null) return NotificationError<DocumentNotifications>.NotificationNotFound(NotificationID);
        return Result<DocumentNotifications>.Success(data);
    }

    public async Task<DocumentNotifications> SaveDocumentNotifications(DocumentNotifications Entity)
    {
        removeWorkQueueCache(Entity.DriverId);
        return await new GenericRepository<DocumentNotifications>(dbContext).Insert(Entity);
    }

    public async Task UpdateDocumentNotifications(DocumentNotifications Entity)
    {
        removeWorkQueueCache(Entity.DriverId);
        await new GenericRepository<DocumentNotifications>(dbContext).Update(Entity);
    }

    private void removeWorkQueueCache(int? driverID)
    {
        cacheService.RemoveAsync($"NotificationUnseen{driverID.Value}");
        cacheService.RemoveAsync($"NotificationAll{driverID.Value}");
    }


}