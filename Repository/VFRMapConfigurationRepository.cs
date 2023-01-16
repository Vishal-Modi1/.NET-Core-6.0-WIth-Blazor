using DataModels.Entities;
using DataModels.VM.Weather;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VFRMapConfigurationRepository : BaseRepository<VFRMapConfiguration>, IVFRMapConfigurationRepository
    {
        private readonly MyContext _myContext;

        public VFRMapConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(VFRMapConfiguration vFRMapConfigurations)
        {
            VFRMapConfiguration existingVFRMapConfiguration = _myContext.VFRMapConfigurations.Where(p => p.UserId == vFRMapConfigurations.UserId).FirstOrDefault();

            if (existingVFRMapConfiguration != null)
            {
                existingVFRMapConfiguration.Width = vFRMapConfigurations.Width;
                existingVFRMapConfiguration.Height = vFRMapConfigurations.Height;
            }
            else
            {
                _myContext.VFRMapConfigurations.Add(vFRMapConfigurations);
            }

            _myContext.SaveChanges();
        }

        public void SetDefault(long userId, short height, short width)
        {
            VFRMapConfiguration data = FindByCondition(p => p.UserId == userId);

            if (data == null)
            {
                data = new VFRMapConfiguration();
            }

            data.UserId = userId;
            data.Height = height;
            data.Width = width;

            SetDefault(data);
        }
    }
}
