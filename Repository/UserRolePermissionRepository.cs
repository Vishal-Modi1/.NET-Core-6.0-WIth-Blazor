using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.UserRolePermission;

namespace Repository
{
    public class UserRolePermissionRepository : IUserRolePermissionRepository
    {
        private MyContext _myContext;

        public List<UserRolePermissionDataVM> GetByRoleId(int roleId, int? companyId)
        {
            using (_myContext = new MyContext())
            {
                List<UserRolePermissionDataVM> userRolePermissionsList = (from userRolePermission in _myContext.UserRolePermissions
                                                                      join userRole in _myContext.UserRoles
                                                                      on userRolePermission.RoleId equals userRole.Id
                                                                      join permission in _myContext.Permissions
                                                                      on userRolePermission.PermissionId equals permission.Id
                                                                      join module in _myContext.ModuleDetails
                                                                      on userRolePermission.ModuleId equals module.Id
                                                                      where userRolePermission.RoleId == roleId
                                                                      && (companyId.GetValueOrDefault() == 0  ? true : userRolePermission.CompanyId == companyId)
                                                                      select new UserRolePermissionDataVM()
                                                                      {
                                                                          Id = userRolePermission.Id,
                                                                          ModuleName = module.Name,
                                                                          RoleId = userRolePermission.RoleId,
                                                                          RoleName = userRole.Name,
                                                                          ActionName = module.ActionName,
                                                                          ControllerName = module.ControllerName,
                                                                          Icon = module.Icon,
                                                                          OrderNo = module.OrderNo,
                                                                          DisplayName = module.DisplayName,
                                                                          ModuleId = module.Id,
                                                                          PermissionId = permission.Id,
                                                                          PermissionType = permission.PermissionType,
                                                                          IsAllowed = userRolePermission.IsAllowed,
                                                                          IsAllowedForMobileApp = userRolePermission.IsAllowedForMobileApp
                                                                      }).OrderBy(p=>p.OrderNo).ToList();

                return userRolePermissionsList;
            }
        }

        public List<UserRolePermissionDataVM> List(UserRolePermissionDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                
                List<UserRolePermissionDataVM> list;

                string sql = $"EXEC dbo.GetUserRolePermissionList '{ datatableParams.SearchText }', { pageNo }, " +
                    $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'," +
                    $"{datatableParams.ModuleId},{datatableParams.RoleId},{datatableParams.CompanyId}";

                list = _myContext.UserRolePermissionList.FromSqlRaw<UserRolePermissionDataVM>(sql).ToList();

                return list;

              }
        }

        public UserRolePermission Update(UserRolePermissionDataVM userRolePermissionVM)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePermission(int id, bool isAllow)
        {
            using (_myContext = new MyContext())
            {
                UserRolePermission userRolePermission = _myContext.UserRolePermissions.Where(p => p.Id == id).FirstOrDefault();

                if (userRolePermission != null)
                {
                    userRolePermission.IsAllowed = isAllow;
                    _myContext.SaveChanges();
                }
            }
        }

        public void UpdatePermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            using (_myContext = new MyContext())
            {
                List<UserRolePermission> userRolePermissionsList = _myContext.UserRolePermissions.Where(p => 
                                                        (userRolePermissionFilterVM.CompanyId == 0 ? true : p.CompanyId == userRolePermissionFilterVM.CompanyId)
                                                        && (userRolePermissionFilterVM.UserRoleId == 0 ? true : p.RoleId == userRolePermissionFilterVM.UserRoleId)
                                                        && (userRolePermissionFilterVM.ModuleId == 0 ? true : p.ModuleId == userRolePermissionFilterVM.ModuleId)
                                                        ).ToList();

                if (userRolePermissionsList.Count() >  0)
                {
                    userRolePermissionsList.ForEach(l => { l.IsAllowed = userRolePermissionFilterVM.IsAllow; });
                    _myContext.SaveChanges();
                }
            }
        }

        public void UpdateMobileAppPermission(int id, bool isAllow)
        {
            using (_myContext = new MyContext())
            {
                UserRolePermission userRolePermission = _myContext.UserRolePermissions.Where(p => p.Id == id).FirstOrDefault();

                if (userRolePermission != null)
                {
                    userRolePermission.IsAllowedForMobileApp = isAllow;
                    _myContext.SaveChanges();
                }
            }
        }

        public void UpdateMobileAppPermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            using (_myContext = new MyContext())
            {
                List<UserRolePermission> userRolePermissionsList = _myContext.UserRolePermissions.Where(p =>
                                                        (userRolePermissionFilterVM.CompanyId == 0 ? true : p.CompanyId == userRolePermissionFilterVM.CompanyId)
                                                        && (userRolePermissionFilterVM.UserRoleId == 0 ? true : p.RoleId == userRolePermissionFilterVM.UserRoleId)
                                                        && (userRolePermissionFilterVM.ModuleId == 0 ? true : p.ModuleId == userRolePermissionFilterVM.ModuleId)
                                                        ).ToList();

                if (userRolePermissionsList.Count() > 0)
                {
                    userRolePermissionsList.ForEach(l => { l.IsAllowedForMobileApp = userRolePermissionFilterVM.IsAllowForMobileApp; });
                    _myContext.SaveChanges();
                }
            }
        }
    }
}
