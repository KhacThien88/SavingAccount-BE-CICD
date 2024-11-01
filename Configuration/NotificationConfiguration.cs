using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(t => t.IdNotification);
            builder.Property(t => t.Content).IsRequired();
        }
    }
}
