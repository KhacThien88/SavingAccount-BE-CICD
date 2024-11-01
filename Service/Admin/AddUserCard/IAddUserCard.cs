using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Admin.AddUserCard
{
    public interface IAddUserCard
    {
        bool AddCard(CardAddModel cam);
    }
}
