using Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.UserRole;
using DataModels.VM.Common;

namespace Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private MyContext _myContext;

        public List<UserRoleVM> List()
        {
            using (_myContext = new MyContext())
            {
                List<UserRoleVM> userRoleList = (from userRole in _myContext.UserRoles
                                                 select new UserRoleVM()
                                                 {
                                                     Id = userRole.Id,
                                                     Name = userRole.Name
                                                 }).ToList();

                return userRoleList;
            }
        }

        public List<DropDownValues> ListDropDownValues(int roleId)
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> userRoleList = (from userRole in _myContext.UserRoles
                                                     where userRole.Priority >= roleId
                                                 select new DropDownValues()
                                                 {
                                                     Id = userRole.Id,
                                                     Name = userRole.Name
                                                 }).ToList();

                return userRoleList;
            }
        }
    }
}
