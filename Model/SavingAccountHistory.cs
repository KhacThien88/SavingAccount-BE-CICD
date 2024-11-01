namespace SavingAccount_BE.Model
{
    public class SavingAccountHistory
    {
        public int Id { get; set; }
        public string IdSavingAccount { get; set; }
        public string IdHistory { get; set; }
        public SavingAccount SavingAccount { get; set; }
        public History History { get; set; }
    }
}
