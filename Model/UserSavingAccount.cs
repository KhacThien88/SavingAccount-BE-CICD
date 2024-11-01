namespace SavingAccount_BE.Model
{
    public class UserSavingAccount
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string IdSavingAccount { get; set; }
        public User User { get; set; }
        public SavingAccount SavingAccount { get; set; }
    }
}
