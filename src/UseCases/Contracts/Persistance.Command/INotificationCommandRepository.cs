namespace UseCases.Contracts.Persistance.Command;

public interface INotificationCommandRepository
{
    Task<Result<DocumentNotifications>> GetNotification(int NotificationID);
    Task<DocumentNotifications> SaveDocumentNotifications(DocumentNotifications Entity);

    Task UpdateDocumentNotifications(DocumentNotifications Entity);
}