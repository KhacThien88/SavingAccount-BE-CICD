using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class SavingAccountConfiguration : IEntityTypeConfiguration<SavingAccount>
    {
        public void Configure(EntityTypeBuilder<SavingAccount> builder)
        {
            builder.ToTable("SavingAccount");
            builder.HasKey(t => t.IdSavingAccount);
            builder.Property(t=>t.Term).IsRequired();
            builder.Property(t => t.Balance).IsRequired();
            builder.Property(t => t.NameOfSavingAccount).IsRequired();
            builder.Property(t => t.DateOpened).IsRequired();
            builder.Property(t => t.Deposits).IsRequired().HasDefaultValue(0);
            builder.Property(t => t.Withdraw).IsRequired().HasDefaultValue(0);
        }
    }
}
