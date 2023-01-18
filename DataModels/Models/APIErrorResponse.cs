using Newtonsoft.Json;

namespace DataModels.Models
{
    public class APIErrorResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
