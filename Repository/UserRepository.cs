using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataModels.VM.Common;
using DataModels.VM.User;
using DataModels.VM.Account;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private MyContext _myContext;

        public User Create(User user)
        {
            using (_myContext = new MyContext())
            {
                _myContext.Users.Add(user);
                _myContext.SaveChanges();

                return user;
            }
        }

        public User Edit(User user)
        {
            using (_myContext = new MyContext())
            {
                User existingDetails = _myContext.Users.Where(p => p.Id == user.Id).FirstOrDefault();

                if (existingDetails == null)
                    return user;

                existingDetails.CompanyId = user.CompanyId;
                existingDetails.DateofBirth = user.DateofBirth;
                existingDetails.Email = user.Email;
                existingDetails.FirstName = user.FirstName;
                existingDetails.LastName = user.LastName;
                existingDetails.Phone = user.Phone;
                existingDetails.RoleId = user.RoleId;
                existingDetails.IsInstructor = user.IsInstructor;
                existingDetails.InstructorTypeId = user.InstructorTypeId;
                existingDetails.Gender = user.Gender;
                existingDetails.ExternalId = user.ExternalId;
                existingDetails.CountryId = user.CountryId;
                existingDetails.ExternalId = user.ExternalId;
                existingDetails.IsSendEmailInvite = user.IsSendEmailInvite;
                existingDetails.IsSendTextMessage = user.IsSendTextMessage;

                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    existingDetails.Password = user.Password;
                }

                existingDetails.Gender = user.Gender;

                existingDetails.UpdatedBy = user.UpdatedBy;
                existingDetails.UpdatedOn = user.UpdatedOn;

                _myContext.SaveChanges();

                return user;
            }
        }

        public bool IsEmailExist(string email)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Users.Where(p => p.Email == email && p.IsDeleted != true).Count() > 0;
            }
        }

        public bool ResetUserPassword(ResetPasswordVM resetPasswordVM)
        {
            using (_myContext = new MyContext())
            {
                User user = (from u in _myContext.Users
                             join token in _myContext.EmailTokens
                             on u.Id equals token.UserId
                             where token.Token == resetPasswordVM.Token
                             select u).FirstOrDefault();
               
                if (user != null)
                {
                    user.Password = resetPasswordVM.Password;
                    _myContext.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public User FindByCondition(Expression<Func<User, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Users.Where(predicate).FirstOrDefault();
            }
        }

        public List<DropDownLargeValues> ListDropdownValuesbyCondition(Expression<Func<User, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Users.Where(predicate).Select(p=> new DropDownLargeValues() {Id = p.Id, Name = p.FirstName + " " + p.LastName }).ToList();
            }
        }

        public List<UserDataVM> List(UserDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<UserDataVM> list;
                string sql = $"EXEC dbo.GetUserList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}',{datatableParams.CompanyId},{datatableParams.RoleId}";

                list = _myContext.UserSearchList.FromSqlRaw<UserDataVM>(sql).ToList();
               
                return list;

            }

        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                User user = _myContext.Users.Where(p => p.Id == id).FirstOrDefault();

                if(user != null)
                {
                    user.IsDeleted = true;
                    _myContext.SaveChanges();
                }
            }
        }

        public void UpdateActiveStatus(int id, bool isActive)
        {
            using (_myContext = new MyContext())
            {
                User user = _myContext.Users.Where(p => p.Id == id).FirstOrDefault();

                if (user != null)
                {
                    user.IsActive = isActive;
                    _myContext.SaveChanges();
                }
            }
        }
    }
}
