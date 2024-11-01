namespace SavingAccount_BE.Model
{
    public class SavingAccount
    {
        public string IdSavingAccount { get; set; }
        public string NameOfSavingAccount { get; set; }
        public string Term { get; set; }
        public double Balance { get; set; }
        public DateTime DateOpened { get; set; }
    
        public double Withdraw {  get; set; }

        public double Deposits { get; set; }

        public ICollection<SavingAccountHistory> SavingAccountHistories { get; set; }

        public ICollection<UserSavingAccount> UserSavingAccounts { get; set; }
    }
}
