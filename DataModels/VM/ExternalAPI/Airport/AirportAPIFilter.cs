using Newtonsoft.Json;

namespace DataModels.VM.ExternalAPI.Airport
{
    public class AirportAPIFilter
    {
        [JsonProperty("api-version")]
        private string ApiVersion { get; set; } = "2016-10-01";

        [JsonProperty("sp")]
        private string SP { get; set; } = " triggers manual run";

        [JsonProperty("sv")]
        private string SV { get; set; } = "1.0";

        [JsonProperty("sig")]
        private string SIG { get; set; } = "VYDJOMsE2kf7zA9WRwb4lR7BhdV6JgLB6zCakQLPqXY";

        [JsonProperty("response")]
        private string Response { get; set; } = "*";

        public string Name { get; set; }

        [JsonProperty("Loc Id")]
        public string LocId { get; set; }

        [JsonProperty("State Id")]
        public string StateId { get; set; }

        [JsonProperty("Facility Type")]
        public string FacilityType { get; set; }

        [JsonProperty("Site Id")]
        public string SiteId { get; set; }

        [JsonProperty("ICAO Id")]
        private string ICAOId { get; set; }
    }
}
