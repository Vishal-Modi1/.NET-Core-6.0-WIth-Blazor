using Configuration;
using DataModels.Constants;
using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using Service.Utilities;
using System;
using System.IO;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.User;
using DataModels.Models;

namespace Service
{
    public class SendMailService : BaseService, ISendMailService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailTokenRepository _emailTokenRepository;
        private readonly MailSettingConfig _mailSettingConfig;
        private readonly ConfigurationSettings _configurationSettings;
        private readonly MailSender _mailSender;

        public SendMailService(IUserRepository userRepository, IEmailTokenRepository emailTokenRepository)
        {
            _userRepository = userRepository;
            _emailTokenRepository = emailTokenRepository;
            _mailSettingConfig = MailSettingConfig.Instance;
            _configurationSettings = ConfigurationSettings.Instance;
            _mailSender = new MailSender();
        }

        public bool NewUserAccountActivation(UserVM userVM, string token)
        {
            try
            {
                string newUserAccountActivationMailBody = GetEmailTemplate(EmailTemplates.NewUserAccountActivationTemplate);
                newUserAccountActivationMailBody = newUserAccountActivationMailBody.Replace("{name}", userVM.FirstName + " " + userVM.LastName);
                newUserAccountActivationMailBody = newUserAccountActivationMailBody.Replace("{username}", userVM.Email);
                newUserAccountActivationMailBody = newUserAccountActivationMailBody.Replace("{password}", userVM.Password);
                newUserAccountActivationMailBody = newUserAccountActivationMailBody.Replace("{link}", $"{userVM.ActivationLink}{token}");

                User user = _userRepository.FindByCondition(p => p.Email == userVM.Email);

                EmailToken emailToken = SaveEmailToken(EmailTypes.AccountActivation, token, user.Id);

                if (emailToken.Id == 0)
                {
                    return false;
                }

                MailSettings mailSettings = GetMailSettings(userVM.Email, "Account Activation", newUserAccountActivationMailBody, "");

                bool isMailSent = _mailSender.SendMail(mailSettings);

                return isMailSent;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public CurrentResponse PasswordReset(string email, string url, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    User user = _userRepository.FindByCondition(p => p.Email == email && p.IsDeleted != true);

                    if (user == null)
                    {
                        CreateResponse(false, HttpStatusCode.NotFound, "Email is not exist");
                        return _currentResponse;
                    }

                    string body = GetEmailTemplate(EmailTemplates.ForgotPasswordTemplate);
                    body = body.Replace("{Link}", url);

                    MailSettings mailSettings = GetMailSettings(email, "Password Reset", body, "");
                    bool isMailSent = _mailSender.SendMail(mailSettings);

                    EmailToken emailToken = SaveEmailToken(EmailTypes.ForgotPassword, token, user.Id);

                    if (emailToken.Id == 0)
                    {
                        CreateResponse(false, HttpStatusCode.NotFound, "Something went wrong. Please try again later.");
                        return _currentResponse;
                    }

                    if (!isMailSent)
                    {
                        CreateResponse(false, HttpStatusCode.NotFound, "Something went wrong. Please try again later.");
                        return _currentResponse;
                    }

                    CreateResponse(true, HttpStatusCode.OK, "Reset Password mail sent successfully");

                    return _currentResponse;
                }
            }
            catch (Exception ex)
            {
                CreateResponse(ex, HttpStatusCode.InternalServerError, ex.Message);
                return _currentResponse;
            }

            return _currentResponse;
        }

        public bool InviteUser(UserVM userVM, string token, long invitedUserId)
        {
            try
            {
                string inviteUserTemplate = GetEmailTemplate(EmailTemplates.InviteUserTemplate);
                inviteUserTemplate = inviteUserTemplate.Replace("{companyName}", userVM.CompanyName);
                inviteUserTemplate = inviteUserTemplate.Replace("{roleName}", userVM.Role);
                inviteUserTemplate = inviteUserTemplate.Replace("{link}", $"{userVM.ActivationLink}{token}");

                User user = _userRepository.FindByCondition(p => p.Email == userVM.Email);

                EmailToken emailToken = SaveEmailToken(EmailTypes.UserInvitation, token, null, invitedUserId);

                if (emailToken.Id == 0)
                {
                    return false;
                }

                MailSettings mailSettings = GetMailSettings(userVM.Email, $"Invitation to join {userVM.CompanyName}", inviteUserTemplate, "");

                bool isMailSent = _mailSender.SendMail(mailSettings);

                return isMailSent;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetEmailTemplate(string templateName)
        {
            try
            {
                var file = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", templateName);

                return System.IO.File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private MailSettings GetMailSettings(string email, string subject, string body, string cc)
        {
            MailSettings mailSettings = new MailSettings();

            mailSettings.Host = _mailSettingConfig.Host;
            mailSettings.Port = _mailSettingConfig.Port;
            mailSettings.From = _mailSettingConfig.FromEmail;
            mailSettings.Password = _mailSettingConfig.Password;
            mailSettings.To = email;
            mailSettings.Subject = subject;
            mailSettings.Body = body;
            mailSettings.IsBodyHTML = true;
            mailSettings.IsEnableSSL = true;
            mailSettings.CC = "vmodi@cendien.com";

            return mailSettings;
        }

        private EmailToken SaveEmailToken(string emailType, string token, long? userId, long? invitedUserId = null)
        {
            EmailToken emailToken = new EmailToken();

            emailToken.EmailType = emailType;
            emailToken.ExpireOn = DateTime.UtcNow.AddDays(_configurationSettings.EmailTokenExpirationDays);
            emailToken.CreatedOn = DateTime.UtcNow;
            emailToken.Token = token;
            emailToken.UserId = userId;
            emailToken.InvitedUserId = invitedUserId;

            emailToken = _emailTokenRepository.Create(emailToken);

            return emailToken;
        }

       
    }
}
