namespace SavingAccount_BE.Model
{
    public class Notification
    {
        public string IdNotification { get; set; }
        public string Content { get; set; }

        public ICollection<UserNotification> userNotifications { get; set; }
    }
}
