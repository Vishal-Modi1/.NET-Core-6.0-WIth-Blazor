using DataModels.VM.User;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace Web.UI.Data.InviteUser
{
    public class InviteUserService
    {
        private readonly HttpCaller _httpCaller;

        public InviteUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<InviteUserDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "inviteuser/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<InviteUserDataVM>();
            }

            List<InviteUserDataVM> invitedUsersList = JsonConvert.DeserializeObject<List<InviteUserDataVM>>(response.Data.ToString());

            return invitedUsersList;
        }

        public async Task<InviteUserVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"inviteuser/getdetails?id={id}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            InviteUserVM inviteUserVM = new InviteUserVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                inviteUserVM = JsonConvert.DeserializeObject<InviteUserVM>(response.Data.ToString());
            }

            return inviteUserVM;
        }

        public async Task<CurrentResponse> AcceptInvitationAsync(DependecyParams dependecyParams, string token)
        {
            dependecyParams.URL = $"inviteuser/acceptinvitation?token={token}";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, InviteUserVM inviteUserVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(inviteUserVM);

            dependecyParams.URL = "inviteuser/create";

            if (inviteUserVM.Id > 0)
            {
                dependecyParams.URL = "inviteuser/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"inviteuser/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

    }
}
