namespace SavingAccount_BE.Model
{
    public class CardHistory
    {
        public int Id { get; set; }
        public string IdCard { get; set; }
        public string IdHistory { get; set; }
        public Card Card { get; set; }
        public History History { get; set; }
    }
}
