using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.VM.Weather
{
    public class VFRMapConfigurationVM
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
        public bool IsApplyToAll { get; set; }
    }
}
