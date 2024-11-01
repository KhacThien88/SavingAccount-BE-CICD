using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class CardHistoryConfiguration : IEntityTypeConfiguration<CardHistory>
    {
        public void Configure(EntityTypeBuilder<CardHistory> builder)
        {
            builder.ToTable("CardHistory");
            builder.HasKey(ch => ch.Id); 
            builder.Property(ch => ch.IdCard).IsRequired();
            builder.Property(ch => ch.IdHistory).IsRequired();

            builder.HasOne(ch => ch.Card)
                   .WithMany(c => c.CardHistories)
                   .HasForeignKey(ch => ch.IdCard);

            builder.HasOne(ch => ch.History)
                   .WithMany(h => h.CardHistories)
                   .HasForeignKey(ch => ch.IdHistory);
        }
    }

}
