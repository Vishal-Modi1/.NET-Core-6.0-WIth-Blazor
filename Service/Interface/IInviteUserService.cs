using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.User;

namespace Service.Interface
{
    public interface IInviteUserService
    {
        CurrentResponse GetDetails(int roleId, long id);

        CurrentResponse Create(InviteUserVM inviteUserVM);

        CurrentResponse IsValidInvite(InviteUserVM inviteUserVM);

        CurrentResponse AcceptInvitation(string token);

        CurrentResponse List(DatatableParams datatableParams);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse Edit(InviteUserVM inviteUserVM);
    }
}
