using DataModels.VM.Common;

namespace FSMAPI.Utilities
{
    public static class UnAuthorizedResponse
    {
        public static CurrentResponse Response()
        {
            return new CurrentResponse() { Data = null,Message = "You are not authorized for this operation", Status = System.Net.HttpStatusCode.Unauthorized};
        }
    }
}
