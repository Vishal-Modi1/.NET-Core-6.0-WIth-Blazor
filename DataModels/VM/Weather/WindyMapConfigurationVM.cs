using System;

namespace DataModels.VM.Weather
{
    public class WindyMapConfigurationVM
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Int16 Width { get; set; }
        public Int16 Height { get; set; }
        public string Wind { get; set; }
        public string Temperature { get; set; }
        public string Forecast { get; set; }
        public bool IsApplyToAll { get; set; }
    }
}
