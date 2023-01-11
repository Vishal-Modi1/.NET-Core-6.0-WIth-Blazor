using System;

namespace DataModels.Entities
{
    public class RadarMapConfiguration
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
    }
}
