using Newtonsoft.Json;
using System.Net;
using DataModels.VM;
using DataModels.VM.Common;

namespace Service
{
    public class BaseService
    {

        public CurrentResponse _currentResponse;
       // public CurrentResponse1 _currentResponse1;

        public BaseService()
        {
            _currentResponse = new CurrentResponse();
          //  _currentResponse1 = new CurrentResponse1();
        }

        //public int LogException(Exception exception)
        //{
        //    return _baseRepository.LogException(exception);
        //}

        //public CurrentResponse CreateResponse(object data, HttpStatusCode statusCode, string message)
        //{
        //    _currentResponse.Data = JsonConvert.SerializeObject(data); 
        //    _currentResponse.Status = statusCode;
        //    _currentResponse.Message = message;

        //    return _currentResponse;
        //}

        public CurrentResponse CreateResponse(object data, HttpStatusCode statusCode, string message)
        {
            _currentResponse.Data = data;
            _currentResponse.Status = statusCode;
            _currentResponse.Message = message;

            return _currentResponse;
        }
    }
}
