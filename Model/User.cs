using SavingAccount_BE.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavingAccount_BE.Model
{
    public class User
    {
        public string IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public ICollection<UserSavingAccount> UserSavingAccounts { get; set; }

        public ICollection<UserCard> UserCards { get; set; }
        public ICollection<User> Relatives { get; set; }

        public ICollection<UserNotification> userNotifications { get; set; }
    }
}
