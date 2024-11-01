using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Seeders
{
    public static class HistoryDatabaseSeeder
    {
        public static void SeedHistory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>().HasData(
                new History() { IdHistory = "1", DateTransfer = new DateTime(2024, 08, 08), Change = 300, Note = "Transfer" },
                new History() { IdHistory = "2", DateTransfer = new DateTime(2024, 07, 07), Change = -200, Note = "Transfer" },
                new History() { IdHistory = "3", DateTransfer = new DateTime(2024, 06, 06), Change = 500, Note = "Transfer" },
                new History() { IdHistory = "4", DateTransfer = new DateTime(2024, 05, 05), Change = 300, Note = "Transfer" },
                new History() { IdHistory = "5", DateTransfer = new DateTime(2024, 04, 04), Change = -300, Note = "Transfer" },
                new History() { IdHistory = "6", DateTransfer = new DateTime(2024, 05, 05), Change = 800, Note = "Transfer" },
                new History() { IdHistory = "7", DateTransfer = new DateTime(2024, 05, 05), Change = 800, Note = "Transfer" },
                new History() { IdHistory = "8", DateTransfer = new DateTime(2024, 05, 05), Change = 800, Note = "Transfer" },
                new History() { IdHistory = "9", DateTransfer = new DateTime(2024, 05, 05), Change = -800, Note = "Transfer" },
                new History() { IdHistory = "10", DateTransfer = new DateTime(2024, 05, 05), Change = -800, Note = "Transfer" },
                new History() { IdHistory = "11", DateTransfer = new DateTime(2024, 05, 05), Change = -800, Note = "Transfer" }
            ); 
        }
    }
}
