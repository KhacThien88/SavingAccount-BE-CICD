using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class HistoryConfiguration : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.ToTable("History");
            builder.HasKey(t => t.IdHistory);
            builder.Property(t => t.DateTransfer).IsRequired();
            builder.Property(t => t.Change).IsRequired();
            builder.Property(t => t.Note).IsRequired();
        }
    }
}
