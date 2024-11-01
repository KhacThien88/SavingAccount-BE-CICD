using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class UserSavingAccountConfiguration : IEntityTypeConfiguration<UserSavingAccount>
    {
        public void Configure(EntityTypeBuilder<UserSavingAccount> builder)
        {
            builder.ToTable("UserSavingAccount");

            // Sử dụng Id làm khóa chính
            builder.HasKey(usa => usa.Id);

            builder.Property(usa => usa.IdUser).IsRequired();
            builder.Property(usa => usa.IdSavingAccount).IsRequired();

            builder.HasOne(usa => usa.User)
                   .WithMany(u => u.UserSavingAccounts)
                   .HasForeignKey(usa => usa.IdUser);

            builder.HasOne(usa => usa.SavingAccount)
                   .WithMany(sa => sa.UserSavingAccounts)
                   .HasForeignKey(usa => usa.IdSavingAccount);

        }
    }
}
