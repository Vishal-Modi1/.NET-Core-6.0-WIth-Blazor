using DataModels.Entities;
using System;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IEmailTokenRepository
    {
        EmailToken Create(EmailToken emailTokens);

        EmailToken FindByCondition(Expression<Func<EmailToken, bool>> predicate);

        bool IsValidToken(string token);

        void UpdateStatus(string token);
    }
}
