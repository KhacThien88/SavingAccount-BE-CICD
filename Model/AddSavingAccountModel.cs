namespace SavingAccount_BE.Model
{
    public class AddSavingAccountModel
    {
        public string IdCard { get; set; }
        public string IdUser { get; set; }

        public string IdSavingAccount { get; set; }
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string AccountName { get; set; }

        public string Password { get; set; }

        public string SavingAccountName { get; set; }

        public string Term { get; set; }

        public double Amount { get; set; }
    }
}
