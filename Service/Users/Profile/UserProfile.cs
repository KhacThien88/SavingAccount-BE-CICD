using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Profile
{
    public class UserProfile : IUserProfile
    {
        private readonly SavingAccountDbContext _dbContext;

        public UserProfile(SavingAccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationUser GetProfile(string id)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null) { 
                return new ApplicationUser();
            }
            return user;

        }
        public bool updateProfile(string id, UserDTO updatedUser)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }
            if (!string.IsNullOrEmpty(updatedUser.FullName)) user.FullName = updatedUser.FullName;
            if (!string.IsNullOrEmpty(updatedUser.PhoneNumber)) user.PhoneNumber = updatedUser.PhoneNumber;
            if (updatedUser.BirthDate.HasValue) user.BirthDate = updatedUser.BirthDate.Value;
            if (!string.IsNullOrEmpty(updatedUser.Province)) user.Province = updatedUser.Province;
            if (!string.IsNullOrEmpty(updatedUser.City)) user.City = updatedUser.City;
            if (!string.IsNullOrEmpty(updatedUser.Nation)) user.Nation = updatedUser.Nation;
            if (!string.IsNullOrEmpty(updatedUser.Base64Image)) user.Base64Image = updatedUser.Base64Image;

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating profile: " + ex.Message);
                return false;
            }
        }
    }
}
