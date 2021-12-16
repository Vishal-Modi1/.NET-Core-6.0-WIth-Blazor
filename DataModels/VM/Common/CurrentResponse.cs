using System.Net;

namespace DataModels.VM.Common
{
    public class CurrentResponse
    {
        public string Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
