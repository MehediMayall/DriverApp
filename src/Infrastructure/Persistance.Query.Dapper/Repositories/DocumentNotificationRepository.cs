namespace Infrastructure.Persistance.Query.Dapper;

public class DocumentNotificationRepository : GenericRepository<DocumentNotifications>, IDocumentNotificationRepository
{
    public DocumentNotificationRepository(IConfiguration configuration):base(configuration)
    {}

    public async Task<Result<IReadOnlyList<DocumentNotifications>>> GetUnseenNotification(DriverID driverID)
    {
        string query = "SELECT TOP(2500) * FROM Driver_Document_Notifications WHERE DriverID = @DriverID AND SeenOn IS NULL;";
        var data = await db.QueryAsync<DocumentNotifications>(query, new{DriverID = driverID.Value.Value});

        if (data.IsNullOrEmpty()) return  NotificationError<IReadOnlyList<DocumentNotifications>>.UnseenNotificationNotFound(driverID);
        return Result<IReadOnlyList<DocumentNotifications>>.Success(data.ToList());
    }

    public async Task<Result<IReadOnlyList<DocumentNotifications>>> GetNotifications(DriverID driverID)
    {
        string query = "SELECT TOP(150) * FROM Driver_Document_Notifications WHERE DriverID = @DriverID ORDER BY CreatedOn DESC;";
        var data = await db.QueryAsync<DocumentNotifications>(query, new{DriverID = driverID.Value.Value});

        if (data.IsNullOrEmpty()) return  NotificationError<IReadOnlyList<DocumentNotifications>>.NotificationNotFound(driverID);
        return Result<IReadOnlyList<DocumentNotifications>>.Success(data.ToList()); 
    }

    public async Task<Result<DocumentNotifications>> GetNotificationSeenStatus(int NotificationID)
    {
        string query = "SELECT  * FROM Driver_Document_Notifications WHERE ID = @NotificationID;";
        var data = await db.QueryAsync<DocumentNotifications>(query, new{NotificationID});

        if (data.IsNullOrEmpty()) return  NotificationError<DocumentNotifications>.NotificationSeenStatusNotFound(NotificationID);
        return Result<DocumentNotifications>.Success(data.FirstOrDefault());
    }
}