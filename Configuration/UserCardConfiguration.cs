using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class UserCardConfiguration : IEntityTypeConfiguration<UserCard>
    {
        public void Configure(EntityTypeBuilder<UserCard> builder)
        {
            builder.ToTable("UserCard");
            builder.HasKey(uc => uc.Id);
            builder.Property(uc => uc.IdUser).IsRequired();
            builder.Property(uc => uc.IdCard).IsRequired();

            builder.HasOne(uc => uc.User)
                   .WithMany(u => u.UserCards)
                   .HasForeignKey(uc => uc.IdUser)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Card)
                   .WithMany(c => c.UserCards)
                   .HasForeignKey(uc => uc.IdCard)
                   .OnDelete(DeleteBehavior.Cascade);
                   
        }
    }
}
