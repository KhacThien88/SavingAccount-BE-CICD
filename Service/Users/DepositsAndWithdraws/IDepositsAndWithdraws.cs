using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Deposits
{
    public interface IDepositsAndWithdraws
    {
        Task<bool> depositsSavingAccount(SavingAccountDeposits savingAccountDeposits);
        Task<bool> withdrawsSavingAccount(SavingAccountWithdraws savingAccountDeposits);
    }
}
