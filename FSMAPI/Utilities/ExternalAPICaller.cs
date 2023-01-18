using System.Net.Http.Headers;

namespace FSMAPI.Utilities
{
    public class ExternalAPICaller
    {
        public async Task<HttpResponseMessage> Get(string url)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var httpClient = new HttpClient(clientHandler))
            {
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage responseObject = await httpClient.GetAsync(url);

                return responseObject;
            }
        }
    }
}
