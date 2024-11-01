using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.SavingAccounts
{
    public class UserSavingAccountService : IUserSavingAccountService
    {
        private readonly SavingAccountDbContext _dbContext;
        private readonly ILogger<UserSavingAccountService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserSavingAccountService(SavingAccountDbContext dbcontext, ILogger<UserSavingAccountService> logger, UserManager<ApplicationUser> userManager)
        {

            _dbContext = dbcontext;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<bool> AddSavingAccountAsync(AddSavingAccountModel addSavingAccountModel)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == addSavingAccountModel.IdUser);
            if (user == null || !await _userManager.CheckPasswordAsync(user, addSavingAccountModel.Password))
            {
                return false;
            }
            var checkValidSavingAccount = _dbContext.SavingAccounts.FirstOrDefault(sa => sa.NameOfSavingAccount == addSavingAccountModel.SavingAccountName);
            var card = _dbContext.Cards.FirstOrDefault(c => c.IdCard == addSavingAccountModel.IdCard);
            if (card == null || card.Balance < addSavingAccountModel.Amount || addSavingAccountModel.Amount < 100000 || checkValidSavingAccount != null)
            {
                return false;
            }
            else
            {
                card.Balance -= addSavingAccountModel.Amount;
                var IdGenerate = Guid.NewGuid().ToString();
                
                _dbContext.Histories.Add(new History()
                {
                    IdHistory = IdGenerate,
                    Change = -(addSavingAccountModel.Amount),
                    DateTransfer = DateTime.UtcNow,
                    Note = "Open Saving Account"
                    
                });
                _dbContext.CardHistories.Add(new CardHistory() { 
                    IdHistory = IdGenerate,
                    IdCard = addSavingAccountModel.IdCard,
                });
            }

            _dbContext.SavingAccounts.Add(new SavingAccount()
            {
                IdSavingAccount = addSavingAccountModel.IdSavingAccount,
                Balance = addSavingAccountModel.Amount,
                DateOpened = DateTime.Now,
                Deposits = addSavingAccountModel.Amount,
                Term = addSavingAccountModel.Term,
                NameOfSavingAccount = addSavingAccountModel.SavingAccountName,
                Withdraw = 0,
            });
            _dbContext.UserSavingAccounts.Add(new UserSavingAccount()
            {
                IdSavingAccount = addSavingAccountModel.IdSavingAccount,
                IdUser = user.Id,
            });
            var IdGenerate_sa = Guid.NewGuid().ToString();
            _dbContext.Histories.Add(new History()
            {
                IdHistory = IdGenerate_sa ,
                Change = addSavingAccountModel.Amount,
                DateTransfer = DateTime.UtcNow,
                Note = "Open Saving Account"
            });
            _dbContext.SavingAccountsHistory.Add(new SavingAccountHistory()
            {
                IdHistory = IdGenerate_sa ,
                IdSavingAccount = addSavingAccountModel.IdSavingAccount
                
            });

            var result = _dbContext.SaveChanges();
            return result > 0;

        }

        public List<SavingAccount> GetListSavingAccounts(string id)
        {
            // Step 1: Get the user ID list first, forcing it to execute with .ToList()
            var userIds = _dbContext.Users
                .Where(u => u.IdUser == id)
                .Select(u => u.IdUser)
                .ToList(); 

            if (!userIds.Any())
            {
                return new List<SavingAccount>();
            }

          
            var hasValidSavingAccounts = _dbContext.UserSavingAccounts
                .Any(usa => userIds.Contains(usa.IdUser));

            if (!hasValidSavingAccounts)
            {
                return new List<SavingAccount>();
            }

            var listSavingAccountIds = _dbContext.UserSavingAccounts
                .Where(usa => userIds.Contains(usa.IdUser))
                .Select(usa => usa.IdSavingAccount)
                .ToList();

            var listSavingAccounts = _dbContext.SavingAccounts
                .Where(sa => listSavingAccountIds.Contains(sa.IdSavingAccount))
                .ToList();

            foreach (var savingAccount in listSavingAccounts)
            {
                var savingAccountHistories = _dbContext.SavingAccountsHistory
                    .Where(sah => sah.IdSavingAccount == savingAccount.IdSavingAccount)
                    .Select(sah => sah.History)
                    .ToList();

                double totalDeposits = savingAccountHistories
                    .Where(history => history.Change > 0)
                    .Sum(history => history.Change);

                double totalWithdraws = savingAccountHistories
                    .Where(history => history.Change < 0)
                    .Sum(history => Math.Abs(history.Change));

                savingAccount.Deposits = totalDeposits;
                savingAccount.Withdraw = totalWithdraws;
            }

            return listSavingAccounts;
        }



        public async Task<bool> updateSavingAccountBalance(string id)
        {
            List<SavingAccount> listsa =  GetListSavingAccounts(id);
            foreach (var item in listsa)
            {
                double days = (DateTime.UtcNow - item.DateOpened).TotalDays;
                var saID = Guid.NewGuid().ToString();
                if (item.Term == "3 months")
                {
                    double count = days % 90;
                    int countInt = Convert.ToInt32(count);
                    item.Balance += countInt * 0.5 / 100 * 3 * item.Balance;
                    if (countInt > 0)
                    {
                        _dbContext.Histories.Add(new History()
                        {
                            Change = countInt * 0.5 / 100 * 3 * item.Balance,
                            DateTransfer = DateTime.Now,
                            IdHistory = saID,
                            Note = "Thanh toan lai"
                        });
                        _dbContext.SavingAccountsHistory.Add(new()
                        {
                            IdHistory = saID,
                            IdSavingAccount = item.IdSavingAccount
                        });
                    }

                }
                else if (item.Term == "6 months")
                {
                    double count = days % 180;
                    int countInt = Convert.ToInt32(count);
                    if (countInt > 0)
                    {
                        item.Balance += countInt * 0.55 / 100 * 6 * item.Balance;
                        _dbContext.Histories.Add(new History()
                        {
                            Change = countInt * 0.55 / 100 * 6 * item.Balance,
                            DateTransfer = DateTime.Now,
                            IdHistory =saID,
                            Note = "Thanh toan lai"
                        });
                        _dbContext.SavingAccountsHistory.Add(new()
                        {
                            IdHistory = saID,
                            IdSavingAccount = item.IdSavingAccount
                        });

                    }
                }
                else
                {
                    double count = (days-30) % 30;
                    int countInt = Convert.ToInt32(count);
                    if (countInt > 0)
                    {
                        item.Balance += countInt * 0.15 / 100 * item.Balance;
                        _dbContext.Histories.Add(new History()
                        {
                            Change = countInt * 0.55 / 100 * 6 * item.Balance,
                            DateTransfer = DateTime.Now,
                            IdHistory = Guid.NewGuid().ToString(),
                            Note = "Thanh toan lai"
                        });
                        _dbContext.SavingAccountsHistory.Add(new()
                        {
                            IdHistory = saID,
                            IdSavingAccount = item.IdSavingAccount
                        });
                    }

                }
            }
            var res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }
    }
}
