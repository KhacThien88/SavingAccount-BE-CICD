using Microsoft.AspNetCore.Identity;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Accounts
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task<string> SignInAsync(SignInModel model);
        Task<bool> VerifyPasswordAsync(UserDTO model);
    }
}
