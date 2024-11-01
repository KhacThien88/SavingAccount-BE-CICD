using SavingAccount_BE.Data;
using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Admin.AddUserCard
{
    public class AddUserCard : IAddUserCard
    {
        private readonly SavingAccountDbContext _dbcontext;

        public AddUserCard(SavingAccountDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public bool AddCard(CardAddModel cam)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.IdUser == cam.idUser);
            if (user == null)
            {
                return false;
            }
            var newCard = new Card
            {
                IdCard = cam.IdCard,
                Balance = 100000,
                CardNumber = cam.CardNumber,
                NameOfCard = cam.NameOfCard,
                TypeCard = cam.TypeOfCard,
                DateOpened = DateTime.Now,
            };
            _dbcontext.Cards.Add(newCard);
            var newClaimCard = new UserCard
            {
                IdCard = cam.IdCard,
                IdUser = cam.idUser
            };
            _dbcontext.UserCards.Add(newClaimCard);
            var result = _dbcontext.SaveChanges();

            return result > 0;
        }

    }
}
