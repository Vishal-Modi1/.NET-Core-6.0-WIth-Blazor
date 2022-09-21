using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

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
                EmailToken emailToken = _emailTokenRepository.FindByCondition(p => p.Token == refreshToken && p.UserId == userId && p.EmailType == EmailTypes.RefreshToken);

                if (emailToken == null)
                {
                    CreateResponse(false, HttpStatusCode.BadRequest, "Refresh token is not valid.");
                }
                else if (emailToken.ExpireOn < DateTime.UtcNow)
                {
                    CreateResponse(false,HttpStatusCode.BadRequest, "Refresh token has been expired.");
                }
                else
                {
                    CreateResponse(true, HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse IsValidToken(string token)
        {
            bool isValidToken = _emailTokenRepository.IsValidToken(token);

            if (isValidToken)
            {
                CreateResponse(isValidToken, HttpStatusCode.OK, "Valid Token");
            }
            else
            {
                CreateResponse(isValidToken, HttpStatusCode.NotFound, "Token is expired");
            }

            return _currentResponse;
        }
    }
}
