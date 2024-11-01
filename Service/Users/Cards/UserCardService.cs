using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Cards
{
    public class UserCardService : IUserCard
    {
        private readonly SavingAccountDbContext _dbContext;

        public UserCardService(SavingAccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Card> GetListCards(string userId)
        {
            var user = _dbContext.Users.Where(u => u.IdUser == userId);
            bool checkCardValid = _dbContext.UserCards
                .Where(uc => user.Select(u => u.IdUser).Contains(uc.IdUser))
                .Any();
            if (user == null && checkCardValid)
            {
                return new List<Card>();
            }
            var cardIds = _dbContext.UserCards
                           .Where(uc => uc.IdUser == userId)
                           .Select(uc => uc.IdCard)
                           .Distinct()
                           .ToList();
            var cards = _dbContext.Cards
                            .Where(c => cardIds.Contains(c.IdCard))
                            .ToList();
            return cards;
        }
    }
}
