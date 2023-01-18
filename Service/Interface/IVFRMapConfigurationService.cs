using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IVFRMapConfigurationService
    {
        CurrentResponse FindByUserId(long userId);

        CurrentResponse SetDefault(VFRMapConfigurationVM vfrMapConfigurationVM);
    }
}
