using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("Card");
            builder.HasKey(t => t.IdCard);
            builder.Property(t => t.CardNumber).IsRequired();
            builder.Property(t => t.Balance).IsRequired();
            builder.Property(t => t.DateOpened).IsRequired();
            builder.Property(t => t.TypeCard).IsRequired();
            builder.Property(t => t.NameOfCard).IsRequired();
        }
    }
}
