using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class RadarMapConfigurationRepository : BaseRepository<RadarMapConfiguration>, IRadarMapConfigurationRepository
    {
        private readonly MyContext _myContext;

        public RadarMapConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(RadarMapConfiguration radarMapConfiguration)
        {
            RadarMapConfiguration existingRadarMapConfiguration = _myContext.RadarMapConfigurations.Where(p => p.UserId == radarMapConfiguration.UserId).FirstOrDefault();

            if (existingRadarMapConfiguration != null)
            {
                existingRadarMapConfiguration.Width = radarMapConfiguration.Width;
                existingRadarMapConfiguration.Height = radarMapConfiguration.Height;
            }
            else
            {
                _myContext.RadarMapConfigurations.Add(radarMapConfiguration);
            }
         
            _myContext.SaveChanges();
        }
    }
}
