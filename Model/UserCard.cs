namespace SavingAccount_BE.Model
{
    public class UserCard
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string IdCard { get; set; }
        public User User { get; set; }
        public Card Card { get; set; }
    }
}
