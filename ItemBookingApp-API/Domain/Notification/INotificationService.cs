namespace ItemBookingApp_API.Domain.Notification
{
    public interface INotificationService<T>
    {
        Task SendNotification(T notificationType);
    }
}
