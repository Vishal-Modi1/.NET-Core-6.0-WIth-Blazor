using Configuration;
using DataModels.Entities;
using Repository.Interface;
using System;
using System.Linq;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        private MyContext _myContext;
        private readonly ConfigurationSettings _configurationSettings;

        public AccountRepository()
        {
            _configurationSettings = ConfigurationSettings.Instance;
        }

        public User GetValidUser(string email, string password)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Users.Where(p => p.Email == email && p.Password == password).FirstOrDefault();
            }
        }

        public bool ActivateAccount(string token)
        {
            using (_myContext = new MyContext())
            {
                EmailToken emailToken = _myContext.EmailTokens.Where(p => p.Token == token && p.ExpireOn >= DateTime.UtcNow).FirstOrDefault();

                if(emailToken == null)
                {
                    return false;
                }

                User user = _myContext.Users.Where(p => p.Id == emailToken.UserId).FirstOrDefault();

                if (user == null)
                    return false;

                emailToken.IsUsed = true;
                user.IsActive = true;
                _myContext.SaveChanges();

                return true;
            }
        }
    }
}
