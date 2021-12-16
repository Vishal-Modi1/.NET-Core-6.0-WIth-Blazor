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

        List<UserDataVM> List(DatatableParams datatableParams);

        void Delete(int id);

        void UpdateActiveStatus(int id, bool isActive);
        bool ResetUserPassword(ResetPasswordVM resetPasswordVM);

    }
}
