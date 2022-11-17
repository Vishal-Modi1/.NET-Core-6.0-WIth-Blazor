using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class InviteUserRepository : BaseRepository<InviteUser>, IInviteUserRepository
    {
        private readonly MyContext _myContext;

        public InviteUserRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<InviteUserDataVM> List(UserDatatableParams datatableParams)
        {
            List<InviteUserDataVM> list;

            string sql = $"EXEC dbo.GetInvitedUsersList '{ datatableParams.SearchText }', " +
                $"{ datatableParams.Start }, {datatableParams.Length},'" +
                $"{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'," +
                $" {datatableParams.CompanyId},{datatableParams.RoleId}";

            list = _myContext.InviteUserData.FromSqlRaw<InviteUserDataVM>(sql).ToList();

            return list;
        }

        public void Delete(long id, long deletedBy)
        {
            InviteUser user = _myContext.InvitedUsers.Where(p => p.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;
                user.DeletedBy = deletedBy;

                _myContext.SaveChanges();
            }
        }

        public InviteUser Edit(InviteUser inviteUser)
        {
            InviteUser existingUserDetails = _myContext.InvitedUsers.Where(p=>p.Id == inviteUser.Id).FirstOrDefault();  

            if(existingUserDetails != null)
            {
                existingUserDetails.RoleId = inviteUser.RoleId;

                _myContext.SaveChanges();
            }

            return inviteUser;
        }
    }
}
