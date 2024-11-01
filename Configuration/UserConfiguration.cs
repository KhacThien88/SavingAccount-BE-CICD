using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(t => t.IdUser);
            builder.HasOne(u => u.ApplicationUser).WithMany()
                .HasForeignKey(u => u.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
