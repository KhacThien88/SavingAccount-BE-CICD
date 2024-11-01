using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Configuration;
using SavingAccount_BE.Model;
using SavingAccount_BE.Seeders;

namespace SavingAccount_BE.Data
{
    public class SavingAccountDbContext : IdentityDbContext<ApplicationUser>
    {
        public SavingAccountDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new SavingAccountConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new CardHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new SavingAccountHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserCardConfiguration());
            modelBuilder.ApplyConfiguration(new UserSavingAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationConfiguration());

            //modelBuilder.SeedHistory();
            //modelBuilder.SeedSavingAccount();
            //modelBuilder.SeedCard();
            //modelBuilder.SeedNotification();
            //modelBuilder.SeedUser();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }

        public DbSet<History> Histories { get; set; }
        public DbSet<Notification> Notifications{ get; set; }
        public DbSet<SavingAccount> SavingAccounts { get; set; }

        public DbSet<SavingAccountHistory> SavingAccountsHistory { get; set; }

        public DbSet<CardHistory> CardHistories { get; set; }

        public DbSet<UserSavingAccount> UserSavingAccounts { get; set; }

        public DbSet<UserCard> UserCards { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<ApplicationUser>ApplicationUsers { get; set; }

    }
}
