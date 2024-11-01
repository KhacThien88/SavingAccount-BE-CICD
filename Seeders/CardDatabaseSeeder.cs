using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Model;

public static class CardDatabaseSeeder
{
    public static void SeedCard(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>().HasData(
            new Card() { IdCard = "1", CardNumber = "123456", Balance = 1000, DateOpened = new DateTime(2022, 01, 01), TypeCard = "Credit", NameOfCard = "Visa" },
            new Card() { IdCard = "2", CardNumber = "789012", Balance = 5000, DateOpened = new DateTime(2022, 02, 01), TypeCard = "Debit", NameOfCard = "MasterCard" },
            new Card() { IdCard = "3", CardNumber = "789045", Balance = 5000, DateOpened = new DateTime(2022, 02, 01), TypeCard = "Debit", NameOfCard = "The Ghi No" }
        );

        modelBuilder.Entity<CardHistory>().HasData(
            new CardHistory() { Id = 1, IdCard = "1", IdHistory = "7" },
            new CardHistory() { Id = 2, IdCard = "1", IdHistory = "8" },
            new CardHistory() { Id = 3, IdCard = "2", IdHistory = "9" },
            new CardHistory() { Id = 4, IdCard = "3", IdHistory = "11" }
        );
    }
}
