using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;

namespace Service
{
    public class EmailTokenService : BaseService, IEmailTokenService
    {
        private readonly IEmailTokenRepository _emailTokenRepository;

        public EmailTokenService(IEmailTokenRepository emailTokenRepository)
        {
            _emailTokenRepository = emailTokenRepository;
        }

        public EmailToken Create(EmailToken emailToken)
        {
            try
            {
                return _emailTokenRepository.Create(emailToken);
            }
            catch (Exception exc)
            {
                return new EmailToken();
            }
        }

        public CurrentResponse ValidateToken(string refreshToken, long userId)
        {
            try
            {
                EmailToken emailToken = _emailTokenRepository.FindByCondition(p => p.Token == refreshToken && p.UserId == userId && p.EmailType == EmailType.RefreshToken);

                if (emailToken == null)
                {
                    CreateResponse(false, System.Net.HttpStatusCode.BadRequest, "Refresh token is not valid.");
                }
                else if (emailToken.ExpireOn < DateTime.UtcNow)
                {
                    CreateResponse(false, System.Net.HttpStatusCode.BadRequest, "Refresh token has been expired.");
                }
                else
                {
                    CreateResponse(true, System.Net.HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(false, System.Net.HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
