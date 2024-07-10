using UseCases.Contracts.Infrastructure;

namespace Infrastructure.Persistance.Query.Dapper;

public class DocumentNotificationRepositoryCached : GenericRepository<DocumentNotifications>, IDocumentNotificationRepository
{
    private readonly DocumentNotificationRepository repo;
    private readonly ICacheService cacheService;

    public DocumentNotificationRepositoryCached(DocumentNotificationRepository repo, IConfiguration configuration, ICacheService cacheService):base(configuration)
    {
        this.repo = repo;
        this.cacheService = cacheService;
    }

    public async Task<Result<IReadOnlyList<DocumentNotifications>>> GetUnseenNotification(DriverID driverID)
    {
        string key = $"NotificationUnseen{driverID.Value.Value}";
        var data = await cacheService.GetAsync<IReadOnlyList<DocumentNotifications>>(key);
        if (data is null || !data.Any()) 
        {
            var result = await repo.GetUnseenNotification(driverID);
            if(result.IsFailure) return result;
            data = result.Value;
        }


        if(data is null || !data.Any()) 
            return NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(driverID);

        await cacheService.SetAsync<IReadOnlyList<DocumentNotifications>>(key,data, TimeSpan.FromMinutes(1), default);
        return Result<IReadOnlyList<DocumentNotifications>>.Success(data);
    }

    public async Task<Result<IReadOnlyList<DocumentNotifications>>> GetNotifications(DriverID driverID)
    {
        string key = $"NotificationAll{driverID.Value.Value}";
        var data = await cacheService.GetAsync<IReadOnlyList<DocumentNotifications>>(key, default);

        if (data is null || !data.Any()) 
        {
            var result = await repo.GetNotifications(driverID);
            if(result.IsFailure) return result;
            data = result.Value;
        }


        if(data is null || !data.Any()) return NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(driverID);
        
        await cacheService.SetAsync<IReadOnlyList<DocumentNotifications>>(key, data, TimeSpan.FromHours(12), default);
        return Result<IReadOnlyList<DocumentNotifications>>.Success(data);
    }

    public async Task<Result<DocumentNotifications>> GetNotificationSeenStatus(int NotificationID)
    {
        return  await repo.GetNotificationSeenStatus( NotificationID);
    }
}