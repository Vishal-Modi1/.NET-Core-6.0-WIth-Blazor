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
using DataModels.VM.UserPreference;
using DataModels.Enums;

namespace Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly MyContext _myContext;

        public UserRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public User Edit(User user)
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

            //TODO : roleid
            //existingDetails.RoleId = user.RoleId;
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

        public bool IsEmailExist(string email)
        {
            return _myContext.Users.Where(p => p.Email == email && p.IsDeleted != true).Count() > 0;
        }

        public bool ResetUserPassword(ResetPasswordVM resetPasswordVM)
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

        public List<DropDownLargeValues> ListDropdownValuesbyCompanyId(int companyId)
        {
            List<DropDownLargeValues> usersList = (from user in _myContext.Users 
                        join userVSCompany in _myContext.UsersVsCompanies 
                        on user.Id equals userVSCompany.UserId
                        where userVSCompany.CompanyId == companyId
                        && user.IsActive == true &&  user.IsDeleted == false
                        select new DropDownLargeValues()
                        {
                            Id = user.Id,
                            Name = user.FirstName + " " + user.LastName
                        }).ToList();

            return usersList;
        }

        public List<User> ListAllbyCompanyId(int companyId)
        {
            List<User> usersList = (from user in _myContext.Users
                                                   join userVSCompany in _myContext.UsersVsCompanies
                                                   on user.Id equals userVSCompany.UserId
                                                   where userVSCompany.CompanyId == companyId
                                                   && user.IsActive == true && user.IsDeleted == false
                                                   select new User()
                                                   {
                                                       Id = user.Id,
                                                       FirstName = user.FirstName,
                                                       LastName = user.LastName,
                                                       Email = user.Email
                                                   }).ToList();

            return usersList;
        }

        public List<UserDataVM> List(UserDatatableParams datatableParams)
        {
            List<UserDataVM> list;

            string sql = $"EXEC dbo.GetUsersList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}',{datatableParams.CompanyId},{datatableParams.RoleId}";

            list = _myContext.UserSearchList.FromSqlRaw<UserDataVM>(sql).ToList();

            return list;
        }

        public void Delete(long id, long deletedBy)
        {
            User user = _myContext.Users.Where(p => p.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;
                user.DeletedBy = deletedBy;

                _myContext.SaveChanges();
            }
        }

        public void UpdateActiveStatus(long id, bool isActive)
        {
            User user = _myContext.Users.Where(p => p.Id == id).FirstOrDefault();

            if (user != null)
            {
                user.IsActive = isActive;
                _myContext.SaveChanges();
            }
        }

        public UserVM FindById(long id, bool isSuperAdmin, bool isInvited, int? companyId)
        {
            UserVM userVM = _myContext.UserDetails.FromSqlRaw("EXECUTE GetUserDetailsById {0},{1},{2},{3}", id, isSuperAdmin, isInvited, companyId.GetValueOrDefault()).AsEnumerable().FirstOrDefault();

            if (userVM != null)
            {
                userVM.UserPreferences = FindPreferenceById(id);
            }

            return userVM;
        }

        public List<UserPreferenceVM> FindPreferenceById(long id)
        {
            List<UserPreference> userPreferences = _myContext.UserPreferences.Where(p => p.UserId == id).ToList();
            List<UserPreferenceVM> userPreferenceVM = new List<UserPreferenceVM>();
            userPreferences.ForEach(p =>
            {
                userPreferenceVM.Add(new UserPreferenceVM()
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    PreferenceType = (PreferenceType)Enum.Parse(typeof(PreferenceType), p.PreferenceType, true),
                    ListPreferencesIds = p.PreferencesIds.Split(new char[] { ',' }).ToList(),

                });
            });

            return userPreferenceVM;
        }

        public bool UpdateImageName(long id, string imageName)
        {
            User existingUser = _myContext.Users.Where(p => p.Id == id).FirstOrDefault();

            if (existingUser != null)
            {
                existingUser.ImageName = imageName;
                _myContext.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
