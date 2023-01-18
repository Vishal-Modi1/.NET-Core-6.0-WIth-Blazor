using DataModels.Entities;

namespace Repository.Interface
{
    public interface IUserAirTrafficControlCenterRepository : IBaseRepository<UserAirTrafficControlCenter>
    {
        void SetDefault(UserAirTrafficControlCenter userAirTrafficControlCenter);
    }
}
