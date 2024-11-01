using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Cards
{
    public interface IUserCard
    {
        List<Card> GetListCards(string userId);
    }
}
