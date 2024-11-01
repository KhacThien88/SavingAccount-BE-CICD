using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Profile
{
    public interface IUserProfile
    {
        ApplicationUser GetProfile(string id);

        bool updateProfile(string id, UserDTO updatedUser);
    }
}
