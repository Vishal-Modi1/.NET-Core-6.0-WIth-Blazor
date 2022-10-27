using Microsoft.AspNetCore.Mvc;
using DataModels.VM.Common;
using DataModels.VM.ExternalAPI.Airport;
using Newtonsoft.Json;
using System.Reflection;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Configuration;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AirportController : BaseAPIController
    {
        private ExternalAPICaller _externalAPICaller { get; set; }

        public AirportController()
        {
            _externalAPICaller = new ExternalAPICaller();
        }

        [HttpPost]
        [Route("listDropdownValues")]
        public async Task<IActionResult> ListDropdownValuesAsync(AirportAPIFilter airportAPIFilter)
        {
            CurrentResponse response = new CurrentResponse();
            List<DropDownGuidValues> dropDownGuidValues = new List<DropDownGuidValues>();

            try
            {
                response.Status = System.Net.HttpStatusCode.OK;
                response.Message = "";

                string url = ConfigurationSettings.Instance.AirportAPIURL;

                PropertyInfo[] properties = typeof(AirportAPIFilter).GetProperties();
                airportAPIFilter.ICAOId = airportAPIFilter.AirportCode;

                foreach (PropertyInfo property in properties)
                {
                    string key = property.Name;
                    string value = Convert.ToString(property.GetValue(airportAPIFilter));

                    if ((key == nameof(AirportAPIFilter.LocId) || key == nameof(AirportAPIFilter.StateId) || key == nameof(AirportAPIFilter.FacilityType)
                        || key == nameof(AirportAPIFilter.SiteId) || key == nameof(AirportAPIFilter.Name) || key == nameof(AirportAPIFilter.ICAOId))
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        foreach (object attr in property.GetCustomAttributes(true))
                        {
                            key = (attr as JsonPropertyAttribute).PropertyName;
                        }

                        url += $"&{key}={value}";
                    }
                }

                HttpResponseMessage responseObject = await _externalAPICaller.Get(url);

                if (!responseObject.IsSuccessStatusCode)
                {
                    response.Status = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong!, Please try again later";

                    return APIResponse(response);
                }

                string jsonString = await responseObject.Content.ReadAsStringAsync();
                AirportViewModel airportDetailsViewModel = JsonConvert.DeserializeObject<AirportViewModel>(jsonString);

                foreach (AirportDetailsViewModel item in airportDetailsViewModel.Value)
                {
                    dropDownGuidValues.Add(new DropDownGuidValues() { Id = item.id, Name = item.ICAOId });
                }

                response.Data = dropDownGuidValues;
            }
            catch (Exception ex)
            {
                response.Status = System.Net.HttpStatusCode.OK;
                response.Message = ex.ToString();
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("isValid")]
        public async Task<IActionResult> IsValid(string airportName)
        {
            CurrentResponse response = new CurrentResponse();
            List<DropDownGuidValues> dropDownGuidValues = new List<DropDownGuidValues>();

            try
            {
                response.Status = System.Net.HttpStatusCode.NotFound;
                response.Message = "Invalid airport";

                string url = $"{ConfigurationSettings.Instance.AirportAPIURL}&Name={airportName}";

                HttpResponseMessage responseObject = await _externalAPICaller.Get(url);

                if (!responseObject.IsSuccessStatusCode)
                {
                    response.Status = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong!, Please try again later";

                    return APIResponse(response);
                }

                string responseJson = await responseObject.Content.ReadAsStringAsync();
                AirportViewModel airportDetailsViewModel = JsonConvert.DeserializeObject<AirportViewModel>(responseJson);

                if (airportDetailsViewModel == null || airportDetailsViewModel.Value.Count() == 0)
                {
                    return APIResponse(response);
                }

                AirportDetailsViewModel airportDetails = airportDetailsViewModel.Value.Where(p => p.Name.ToLower() == airportName.ToLower()).FirstOrDefault();

                if (airportDetails != null)
                {
                    response.Status = System.Net.HttpStatusCode.OK;
                    response.Message = "";
                    response.Data = airportDetails;
                }
            }
            catch (Exception ex)
            {
                response.Status = System.Net.HttpStatusCode.OK;
                response.Message = ex.ToString();
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("findByName")]
        public async Task<IActionResult> FindByName(string airportName)
        {
            CurrentResponse response = new CurrentResponse();
            List<DropDownGuidValues> dropDownGuidValues = new List<DropDownGuidValues>();

            try
            {
                response.Status = System.Net.HttpStatusCode.NotFound;
                response.Message = "Invalid airport";

                string url = $"{ConfigurationSettings.Instance.AirportAPIURL}&ICAO Id={airportName}";

                HttpResponseMessage responseObject = await _externalAPICaller.Get(url);

                if (!responseObject.IsSuccessStatusCode)
                {
                    response.Status = System.Net.HttpStatusCode.InternalServerError;
                    response.Message = "Something went wrong!, Please try again later";

                    return APIResponse(response);
                }

                string responseJson = await responseObject.Content.ReadAsStringAsync();
                AirportViewModel airportDetailsViewModel = JsonConvert.DeserializeObject<AirportViewModel>(responseJson);

                if (airportDetailsViewModel == null || airportDetailsViewModel.Value.Count() == 0)
                {
                    return APIResponse(response);
                }

                var airportDetails = airportDetailsViewModel.Value.Where(p => p.ICAOId.ToLower() == airportName.ToLower()).FirstOrDefault();

                if (airportDetails != null)
                {
                    response.Status = System.Net.HttpStatusCode.OK;
                    response.Message = "";
                    response.Data = airportDetails;
                }
            }
            catch (Exception ex)
            {
                response.Status = System.Net.HttpStatusCode.OK;
                response.Message = ex.ToString();
            }

            return APIResponse(response);
        }
    }
}
