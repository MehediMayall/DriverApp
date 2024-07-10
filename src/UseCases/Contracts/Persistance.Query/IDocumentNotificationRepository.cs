namespace UseCases.Contracts.Persistance.Query;

public interface IDocumentNotificationRepository: IAsyncRepository<DocumentNotifications>
{
    Task<Result<IReadOnlyList<DocumentNotifications>>> GetUnseenNotification(DriverID driverID);

    Task<Result<IReadOnlyList<DocumentNotifications>>> GetNotifications(DriverID driverID);

    Task<Result<DocumentNotifications>> GetNotificationSeenStatus(int NotificationID);

    // Task UpdateDocumentNotifications(DocumentNotifications Entity);

    // Task<DocumentNotifications> SaveDocumentNotifications(DocumentNotifications Entity);
}