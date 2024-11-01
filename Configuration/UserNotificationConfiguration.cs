using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotification");
            builder.HasKey(un => un.Id);
            builder.Property(un => un.IdUser).IsRequired();
            builder.Property(un => un.IdNotification).IsRequired();
            builder.HasOne(un => un.User)
                   .WithMany(u => u.userNotifications)
                   .HasForeignKey(un => un.IdUser)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(un => un.Notification)
                   .WithMany(n => n.userNotifications)
                   .HasForeignKey(un => un.IdNotification)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
