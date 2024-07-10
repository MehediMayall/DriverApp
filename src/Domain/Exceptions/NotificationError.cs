namespace Domain.Exceptions;

public static class NotificationError<DocumentNotifications> where DocumentNotifications : class
{     
    public static Error<DocumentNotifications> UnseenNotificationNotFound(DriverID driverID)
    {
        return new($"Couldn't find any unseen notification for driver id: {driverID.Value}", "DocumentNotificationRepository.GetNotifications");
    }
    public static Error<DocumentNotifications> NotificationNotFound(DriverID driverID)
    {
        return new( $"Couldn't find any notification for driver id: {driverID.Value}","DocumentNotificationRepository.GetNotifications");
    }
    public static Error<DocumentNotifications> NotificationNotFound(int NotificationID)
    {
        return new($"Couldn't find any notification for driver id: {NotificationID}", "DocumentNotificationRepository.GetNotifications");
    }
    public static Error<DocumentNotifications> NotificationSeenStatusNotFound(int  NotificationID)
    {
        return new($"Couldn't find any notification status for NotificationID id: {NotificationID}", "DocumentNotificationRepository.GetNotifications");
    }
}