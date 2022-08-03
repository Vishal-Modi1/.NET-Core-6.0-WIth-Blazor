using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.User;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IInviteUserRepository : IBaseRepository<InviteUser>
    {
        List<InviteUserDataVM> List(DatatableParams datatableParams);

        void Delete(long id, long deletedBy);

        InviteUser Edit(InviteUser inviteUser);
    }
}
