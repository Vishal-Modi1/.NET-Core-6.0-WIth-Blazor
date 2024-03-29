﻿using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;

namespace Repository.Interface
{
    public interface IModuleDetailsRepository
    {
        List<ModuleDetailsVM> List();

        List<DropDownValues> ListDropDownValues();

        ModuleDetail FindByName(string name);
    }
}
