using DataModels.Entities;

namespace Repository.Interface
{
    public interface IAccountRepository
    {
        User GetValidUser(string email, string password);

        bool ActivateAccount(string token);
    }
}
