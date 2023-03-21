using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class NOAARadarMapConfigurationRepository : BaseRepository<NOAARadarMapConfiguration>, INOAARadarMapConfigurationRepository
    {
        private readonly MyContext _myContext;

        public NOAARadarMapConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(NOAARadarMapConfiguration nOAARadarMapConfiguration)
        {
            NOAARadarMapConfiguration existingNOAARadarMapConfiguration = _myContext.NOAARadarMapConfigurations.Where(p => p.UserId == nOAARadarMapConfiguration.UserId).FirstOrDefault();

            if (existingNOAARadarMapConfiguration != null)
            {
                existingNOAARadarMapConfiguration.Width = nOAARadarMapConfiguration.Width;
                existingNOAARadarMapConfiguration.Height = nOAARadarMapConfiguration.Height;
            }
            else
            {
                _myContext.NOAARadarMapConfigurations.Add(nOAARadarMapConfiguration);
            }
         
            _myContext.SaveChanges();
        }

        public void SetDefault(long userId, short height, short width)
        {
            NOAARadarMapConfiguration data = FindByCondition(p => p.UserId == userId);

            if (data == null)
            {
                data = new NOAARadarMapConfiguration();
            }

            data.UserId = userId;
            data.Height = height;
            data.Width = width;

            SetDefault(data);
        }
    }
}
