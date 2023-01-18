using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class UserAirTrafficControlCenterRepository : BaseRepository<UserAirTrafficControlCenter>, IUserAirTrafficControlCenterRepository
    {
        private readonly MyContext _myContext;

        public UserAirTrafficControlCenterRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(UserAirTrafficControlCenter userAirTrafficControlCenter)
        {
            UserAirTrafficControlCenter existingUserAirTrafficControlCenter = _myContext.UsersAirTrafficControlCenter.Where(p => p.UserId == userAirTrafficControlCenter.UserId).FirstOrDefault();

            if (existingUserAirTrafficControlCenter != null)
            {
                existingUserAirTrafficControlCenter.AirTrafficControlCenterId = userAirTrafficControlCenter.AirTrafficControlCenterId;
            }
            else
            {
                _myContext.UsersAirTrafficControlCenter.Add(userAirTrafficControlCenter);
            }
         
            _myContext.SaveChanges();
        }
    }
}
