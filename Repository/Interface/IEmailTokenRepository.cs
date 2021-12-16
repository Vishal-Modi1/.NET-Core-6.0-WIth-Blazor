using DataModels.Entities;


namespace Repository.Interface
{
    public interface IEmailTokenRepository
    {
        EmailToken Create(EmailToken emailTokens);
    }
}
