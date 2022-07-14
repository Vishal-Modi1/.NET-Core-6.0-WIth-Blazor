using System.Net.Http.Headers;

namespace FSM.Blazor.Pages.BaseComponent
{
    public  class BaseComponent
    {
        public async Task<bool> IsTokenValid(IHttpClientFactory _httpClientFactory, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"account/IsTokenValid");
                request.Headers.Clear();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var client = _httpClientFactory.CreateClient("FSMAPI");

                HttpResponseMessage httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return false;
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
