using DataModels.Entities;
using Repository.Interface;
using System.Linq;
using DataModels.VM.MyAccount;

namespace Repository
{
    public class MyAccountRepository : IMyAccountRepository
    {
        private MyContext _myContext;

        public bool ChangePassword(ChangePasswordVM changePasswordVM)
        {
            using (_myContext = new MyContext())
            {
                User user = _myContext.Users.Where(p => p.Id == changePasswordVM.UserId).FirstOrDefault();
                
                if(user == null)
                {
                    return false;
                }

                user.Password = changePasswordVM.NewPassword;
                _myContext.SaveChanges();

                return true;
            }
        }

        public bool IsValidOldPassword(string password, int userId)
        {
            using(_myContext = new MyContext())
            {
                bool isValidOldPassword = _myContext.Users.Where(p => p.Password == password && p.Id == userId).Count() > 0;

                return isValidOldPassword;
            }
        }
    }
}
