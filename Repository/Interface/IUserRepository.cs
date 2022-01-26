using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.User;
using DataModels.VM.Common;
using DataModels.VM.Account;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        User Create(User user);

        bool IsEmailExist(string email);

        User FindByCondition(Expression<Func<User, bool>> predicate);

        User Edit(User user);

        List<UserDataVM> List(UserDatatableParams datatableParams);

        void Delete(long id);

        void UpdateActiveStatus(long id, bool isActive);

        bool ResetUserPassword(ResetPasswordVM resetPasswordVM);

        List<DropDownLargeValues> ListDropdownValuesbyCondition(Expression<Func<User, bool>> predicate);

    }
}
