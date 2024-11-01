namespace SavingAccount_BE.Model
{
    public class History
    {
        public string IdHistory { get; set; }
        public double Change { get; set; }
        public DateTime DateTransfer { get; set; }
        public string Note { get; set; }

        // Sử dụng bảng phụ để quản lý nhiều Card
        public ICollection<CardHistory> CardHistories { get; set; }

        public ICollection<SavingAccountHistory> SavingAccountHistories { get; set; }
    }
}
