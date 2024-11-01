
using Microsoft.AspNetCore.Identity;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;
using SavingAccount_BE.Service.Users.SavingAccounts;

namespace SavingAccount_BE.Service.Users.Deposits
{
    public class DepositsAndWithdraws : IDepositsAndWithdraws
    {
        private readonly SavingAccountDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserSavingAccountService _userSavingAccountService;

        public DepositsAndWithdraws(SavingAccountDbContext savingAccountDbContext , UserManager<ApplicationUser> userManager , IUserSavingAccountService userSavingAccountService) {
            _dbContext = savingAccountDbContext;
            _userManager = userManager;
            _userSavingAccountService = userSavingAccountService;
        }
        public async Task<bool> depositsSavingAccount(SavingAccountDeposits savingAccountDeposits)
        {
            var sa = _dbContext.SavingAccounts.FirstOrDefault(sa => sa.NameOfSavingAccount == savingAccountDeposits.NameOfSavingAccount);
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == savingAccountDeposits.IdUser);
            if (sa == null ||!await _userManager.CheckPasswordAsync(user , savingAccountDeposits.password)) {
                return false; 
            }
            var card = _dbContext.Cards.FirstOrDefault(c => c.IdCard == savingAccountDeposits.IdCard);
            if (card == null || card.Balance < savingAccountDeposits.Amount || savingAccountDeposits.Amount < 100000)
            {
                return false;
            }
            await _userSavingAccountService.updateSavingAccountBalance(savingAccountDeposits.IdUser);
            var IdGenerate_sa = Guid.NewGuid().ToString();
            var IdGenerate_ca = Guid.NewGuid().ToString();
            _dbContext.Histories.Add(new History()
            {
                Change = savingAccountDeposits.Amount,
                DateTransfer = DateTime.UtcNow,
                Note = "Deposits",
                IdHistory = IdGenerate_sa
            });
            _dbContext.Histories.Add(new History()
            {
                Change = -savingAccountDeposits.Amount,
                DateTransfer = DateTime.UtcNow,
                Note = "Deposits in Saving Account",
                IdHistory = IdGenerate_ca
            });
            _dbContext.SavingAccountsHistory.Add(new SavingAccountHistory() { 
                IdHistory = IdGenerate_sa,
                IdSavingAccount = sa.IdSavingAccount,
            });
            _dbContext.CardHistories.Add(new CardHistory()
            {
                IdHistory = IdGenerate_ca,
                IdCard = card.IdCard
            });
            sa.Balance += savingAccountDeposits.Amount;
            card.Balance -= savingAccountDeposits.Amount;
            var res = await _dbContext.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> withdrawsSavingAccount(SavingAccountWithdraws savingAccountWithdraws)
        {
            
            var sa = _dbContext.SavingAccounts.FirstOrDefault(sa => sa.NameOfSavingAccount == savingAccountWithdraws.NameOfSavingAccount);
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == savingAccountWithdraws.IdUser);
            if (sa == null || !await _userManager.CheckPasswordAsync(user, savingAccountWithdraws.password))
            {
                return false;
            }
            if ((DateTime.UtcNow - sa.DateOpened).TotalDays < 15)
            {
                return false;
            }
            await _userSavingAccountService.updateSavingAccountBalance(savingAccountWithdraws.IdUser);
            var IdGenerate_sa = Guid.NewGuid().ToString();
            if(sa.Term == "No limited" && sa.Balance < savingAccountWithdraws.Amount)
            {
                return false;
            }
            _dbContext.Histories.Add(new History()
            {
                Change = -savingAccountWithdraws.Amount,
                DateTransfer = DateTime.UtcNow,
                Note = "Withdraw money",
                IdHistory = IdGenerate_sa
            });
          
            _dbContext.SavingAccountsHistory.Add(new SavingAccountHistory()
            {
                IdHistory = IdGenerate_sa,
                IdSavingAccount = sa.IdSavingAccount,
            });
           
            sa.Balance -= savingAccountWithdraws.Amount;
            var res = await _dbContext.SaveChangesAsync();
            return res > 0;
        }
    }
}
