using System;
using System.Net;
using DataModels.VM.Common;

namespace Service.Middleware
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public RestException(CurrentResponse response)
        {
            Code = response.Status;
            Errors = response.Message;
        }

        public HttpStatusCode Code { get; }
        public object Errors { get; }
    }
}
