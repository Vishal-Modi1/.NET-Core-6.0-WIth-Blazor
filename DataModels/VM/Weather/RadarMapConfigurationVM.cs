using System;

namespace DataModels.VM.Weather
{
    public class RadarMapConfigurationVM
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
        public bool IsApplyToAll { get; set; }
    }
}
