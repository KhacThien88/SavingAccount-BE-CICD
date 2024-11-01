using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class SavingAccountHistoryConfiguration : IEntityTypeConfiguration<SavingAccountHistory>
    {
        public void Configure(EntityTypeBuilder<SavingAccountHistory> builder)
        {
            builder.ToTable("SavingAccountHistory");
            builder.HasKey(sah => sah.Id); 
            builder.Property(sah => sah.IdSavingAccount).IsRequired();
            builder.Property(sah => sah.IdHistory).IsRequired();
            builder.HasOne(sah => sah.SavingAccount)
                   .WithMany(sa => sa.SavingAccountHistories)
                   .HasForeignKey(sah => sah.IdSavingAccount);
            builder.HasOne(sah => sah.History)
                   .WithMany(h => h.SavingAccountHistories)
                   .HasForeignKey(sah => sah.IdHistory);
        }
    }

}
