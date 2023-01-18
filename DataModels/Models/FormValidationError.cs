using Newtonsoft.Json;

namespace DataModels.Models
{
    public class FormValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public FormValidationError(string field,  string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

}
