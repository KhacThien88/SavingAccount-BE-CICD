namespace SavingAccount_BE.Model
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string IdNotification { get; set; }

        // Quan hệ với User và Card
        public User User { get; set; }
        public Notification Notification { get; set; }
    }
}
