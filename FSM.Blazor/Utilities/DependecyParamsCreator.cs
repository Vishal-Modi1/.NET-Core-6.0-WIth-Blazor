using Microsoft.AspNetCore.Components.Authorization;

namespace FSM.Blazor.Utilities
{
    public class DependecyParamsCreator
    {
        public static DependecyParams Create(IHttpClientFactory httpClientFactory, 
            string url, string jsonData, AuthenticationStateProvider authenticationStateProvider)
        {
            DependecyParams dependecyParams = new DependecyParams();

            dependecyParams.HttpClientFactory = httpClientFactory;
            dependecyParams.AuthenticationStateProvider = authenticationStateProvider;
            dependecyParams.URL = url;
            dependecyParams.JsonData = jsonData;

            return dependecyParams;
        }
    }

    public class DependecyParams
    {
        public IHttpClientFactory HttpClientFactory { get; set; }

        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public string URL { get; set; }

        public string JsonData { get; set; }
    }
}
