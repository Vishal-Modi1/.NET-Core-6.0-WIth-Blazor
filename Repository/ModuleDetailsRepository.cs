using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.UserRolePermission;
using DataModels.VM.Common;

namespace Repository
{
    public class ModuleDetailsRepository : IModuleDetailsRepository
    {
        private MyContext _myContext;

        public List<ModuleDetailsVM> List()
        {
            using (_myContext = new MyContext())
            {
                List<ModuleDetailsVM> moduleDetailsList = (from moduleDetail in _myContext.ModuleDetails
                                                           select new ModuleDetailsVM
                                                           {
                                                               Id = moduleDetail.Id,
                                                               Name = moduleDetail.Name

                                                           }).ToList();

                return moduleDetailsList;
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> moduleList = (from module in _myContext.ModuleDetails
                                                     select new DropDownValues()
                                                     {
                                                         Id = module.Id,
                                                         Name = module.DisplayName
                                                     }).ToList();

                return moduleList;
            }
        }
    }
}
