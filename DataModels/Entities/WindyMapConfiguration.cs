using System;

namespace DataModels.Entities
{
    public class WindyMapConfiguration
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
        public string Wind { get; set; }
        public string Temperature { get; set; }
        public string Forecast { get; set; }
    }
}
