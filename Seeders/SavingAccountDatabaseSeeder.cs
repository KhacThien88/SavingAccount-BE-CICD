using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Model;

public static class SavingAccountDatabaseSeeder
{
    public static void SeedSavingAccount(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SavingAccount>().HasData(
            new SavingAccount() { IdSavingAccount = "1", DateOpened = new DateTime(2022, 08, 08), Balance = 300000, NameOfSavingAccount = "Đi chơi lễ", Term = "3 months" },
            new SavingAccount() { IdSavingAccount = "2", DateOpened = new DateTime(2022, 09, 09), Balance = 500000, NameOfSavingAccount = "Ăn tết", Term = "6 months" },
            new SavingAccount() { IdSavingAccount = "3", DateOpened = new DateTime(2022, 09, 09), Balance = 500000, NameOfSavingAccount = "Đi đà lạt", Term = "6 months" }
        );

        modelBuilder.Entity<SavingAccountHistory>().HasData(
            new SavingAccountHistory() { Id = 1, IdSavingAccount = "1", IdHistory = "2" },
            new SavingAccountHistory() { Id = 2, IdSavingAccount = "1", IdHistory = "5" },
            new SavingAccountHistory() { Id = 3, IdSavingAccount = "1", IdHistory = "6" },
            new SavingAccountHistory() { Id = 4, IdSavingAccount = "2", IdHistory = "4" },
            new SavingAccountHistory() { Id = 5, IdSavingAccount = "3", IdHistory = "10"}
        );
    }
}
