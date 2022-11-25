using DataModels.Entities;
using Repository.Interface;

namespace Repository
{
    public class AirTrafficControlCenterRepository : BaseRepository<AirTrafficControlCenter>, IAirTrafficControlCenterRepository
    {
        private readonly MyContext _myContext;

        public AirTrafficControlCenterRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }
    }
}
