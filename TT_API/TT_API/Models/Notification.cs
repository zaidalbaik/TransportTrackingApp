namespace TTSAPI.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? NotificationType { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public User User { get; set; }
    }
}
