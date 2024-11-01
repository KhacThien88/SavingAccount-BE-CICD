using SavingAccount_BE.Model;

namespace SavingAccount_BE.Service.Users.Histories
{
    public interface IUserHistory
    {
        List<History> GetHistory(string id);
    }
}
