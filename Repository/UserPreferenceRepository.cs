using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private MyContext _myContext;

        public UserPreference Create(UserPreference userPreference)
        {
            using (_myContext = new MyContext())
            {
                UserPreference existingPreference  = _myContext.UserPreferences.Where(p=>p.UserId == userPreference.UserId && userPreference.PreferenceType == userPreference.PreferenceType).FirstOrDefault();

                if(existingPreference != null)
                {
                    _myContext.UserPreferences.Remove(existingPreference);
                }

                _myContext.UserPreferences.Add(userPreference);
                _myContext.SaveChanges();

                return userPreference;
            }
        }
    }
}
