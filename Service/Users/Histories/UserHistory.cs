using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Histories
{
    public class UserHistory : IUserHistory
    {
        private readonly SavingAccountDbContext _dbContext;

        public UserHistory(SavingAccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<History> GetHistory(string id)
        {
            var user = _dbContext.Users.Where(u => u.IdUser == id);

            bool checkCardValid = _dbContext.UserCards
                .Where(uc => user.Select(u => u.IdUser).Contains(uc.IdUser))
                .Any();

            bool checkSavingAccountValid = _dbContext.UserSavingAccounts
                .Where(usa => user.Select(u => u.IdUser).Contains(usa.IdUser))
                .Any();

            if (user == null && (checkCardValid || checkSavingAccountValid))
            {
                return new List<History>();
            }

            var userCard = _dbContext.UserCards
                .Where(uc => user.Select(u => u.IdUser).Contains(uc.IdUser));
            var cards = _dbContext.Cards.Where(c =>userCard.Select(uc => uc.IdCard).Contains(c.IdCard)).ToList();
            var cardHistoryIds = _dbContext.CardHistories
              .Where(ch => cards.Select(c => c.IdCard).Contains(ch.IdCard))
              .Select(ch => ch.IdHistory)
              .Distinct()
              .ToList();
            var userSavingAccounts = _dbContext.UserSavingAccounts
                .Where(usa => user.Select(u => u.IdUser).Contains(usa.IdUser));
            var savingAccounts = _dbContext.SavingAccounts.Where(sa => userSavingAccounts.Select(usa => usa.IdSavingAccount).Contains(sa.IdSavingAccount)).ToList();
            var savingAccountHistoryIds = _dbContext.SavingAccountsHistory
                .Where(sah => savingAccounts.Select(sa => sa.IdSavingAccount).Contains(sah.IdSavingAccount))
                .Select(sah => sah.IdHistory)
                .Distinct()
                .ToList();

            var combinedHistoryIds = cardHistoryIds.Concat(savingAccountHistoryIds).Distinct().ToList();
            var histories = _dbContext.Histories.Where(h => combinedHistoryIds.Contains(h.IdHistory)).ToList();
            return histories;
        }
    }
}
