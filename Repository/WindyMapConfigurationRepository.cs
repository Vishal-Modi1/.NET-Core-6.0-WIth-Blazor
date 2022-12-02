using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class WindyMapConfigurationRepository : BaseRepository<WindyMapConfiguration>, IWindyMapConfigurationRepository
    {
        private readonly MyContext _myContext;

        public WindyMapConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(WindyMapConfiguration windyMapConfiguration)
        {
            WindyMapConfiguration existingWindyMapConfiguration = _myContext.WindyMapConfigurations.Where(p => p.UserId == windyMapConfiguration.UserId).FirstOrDefault();

            if (existingWindyMapConfiguration != null)
            {
                existingWindyMapConfiguration.Width = windyMapConfiguration.Width;
                existingWindyMapConfiguration.Height = windyMapConfiguration.Height;
                existingWindyMapConfiguration.Wind = windyMapConfiguration.Wind;
                existingWindyMapConfiguration.Forecast = windyMapConfiguration.Forecast;
                existingWindyMapConfiguration.Temperature = windyMapConfiguration.Temperature;
            }
            else
            {
                _myContext.WindyMapConfigurations.Add(windyMapConfiguration);
            }
         
            _myContext.SaveChanges();
        }
    }
}
