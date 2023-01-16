using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IVFRMapConfigurationRepository : IBaseRepository<VFRMapConfiguration>
    {
        void SetDefault(VFRMapConfiguration vFRMapConfigurations);

        void SetDefault(long userId, short height, short width);
    }
}
