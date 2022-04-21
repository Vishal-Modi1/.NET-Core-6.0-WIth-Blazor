using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class EmailTokenRepository : IEmailTokenRepository
    {
        private MyContext _myContext;

        public EmailToken Create(EmailToken emailToken)
        {
            using (_myContext = new MyContext())
            {
                List<EmailToken> existingEmailTokens = _myContext.EmailTokens.Where(p => p.EmailType == emailToken.EmailType && p.UserId == emailToken.UserId).ToList();

                _myContext.EmailTokens.RemoveRange(existingEmailTokens);

                _myContext.EmailTokens.Add(emailToken);
                _myContext.SaveChanges();

                return emailToken;
            }
        }

        public EmailToken FindByCondition(Expression<Func<EmailToken, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                EmailToken emailToken = _myContext.EmailTokens.Where(predicate).FirstOrDefault();

                return emailToken;
            }
        }
    }
}
