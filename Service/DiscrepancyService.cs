using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq.Expressions;
using GlobalUtilities;
using System.Linq;
using DataModels.Enums;

namespace Service
{
    public class DiscrepancyService : BaseService, IDiscrepancyService
    {
        private readonly IDiscrepancyRepository _discrepancyRepository;
        private readonly IDiscrepancyHistoryRepository _discrepancyHistoryRepository;
        private readonly IDiscrepancyStatusRepository _discrepancyStatusRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISendMailService _sendMailService;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IEmailConfigurationRepository _emailConfigurationRepository;

        public DiscrepancyService(IDiscrepancyRepository discrepancyRepository,
            IUserRepository userRepository, ISendMailService sendMailService,
            IDiscrepancyHistoryRepository discrepancyHistoryRepository,
            IDiscrepancyStatusRepository discrepancyStatusRepository,
            IAircraftRepository aircraftRepository, IEmailConfigurationRepository emailConfigurationRepository)
        {
            _discrepancyRepository = discrepancyRepository;
            _discrepancyHistoryRepository = discrepancyHistoryRepository;
            _discrepancyStatusRepository = discrepancyStatusRepository;
            _userRepository = userRepository;
            _sendMailService = sendMailService;
            _aircraftRepository = aircraftRepository;
            _emailConfigurationRepository = emailConfigurationRepository;
        }

        public CurrentResponse Create(DiscrepancyVM discrepancyVM, string timezone)
        {
            Discrepancy discrepancy = ToDataObject(discrepancyVM);

            try
            {
                if (discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.Verified_PedingtoRepair ||
                    discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.New_Pending ||
                    discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.Unable_To_Verify)
                {
                    discrepancy.IsActive = true;
                }

                discrepancy = _discrepancyRepository.Create(discrepancy);
                ManageHistory(new Discrepancy(), discrepancy);

                discrepancyVM = ToBusinessObject(discrepancy);
                discrepancyVM.DiscrepancyHistoryVM = _discrepancyHistoryRepository.List(discrepancyVM.Id);

                SendScheduleMail(discrepancyVM, true, timezone);

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "Discrepancy added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(DiscrepancyVM discrepancyVM, string timezone)
        {
            Discrepancy discrepancy = ToDataObject(discrepancyVM);

            try
            {
                if (discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.Verified_PedingtoRepair ||
                   discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.New_Pending ||
                   discrepancy.DiscrepancyStatusId == (int)DataModels.Enums.DiscrepancyStatus.Unable_To_Verify)
                {
                    discrepancy.IsActive = true;
                }

                Discrepancy oldDiscrepancy = _discrepancyRepository.FindByCondition(p => p.Id == discrepancyVM.Id);
                discrepancy = _discrepancyRepository.Edit(discrepancy);
                
                ManageHistory(oldDiscrepancy, discrepancy);

                discrepancyVM = ToBusinessObject(discrepancy);
                discrepancyVM.DiscrepancyHistoryVM = _discrepancyHistoryRepository.List(discrepancyVM.Id);

                SendScheduleMail(discrepancyVM, false, timezone);

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "Discrepancy updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public Discrepancy FindByCondition(Expression<Func<Discrepancy, bool>> predicate)
        {
            return _discrepancyRepository.FindByCondition(predicate);
        }

        public CurrentResponse GetDetails(long id)
        {
            try
            {
                Discrepancy discrepancy = _discrepancyRepository.FindByCondition(p => p.Id == id);
                DiscrepancyVM discrepancyVM = new DiscrepancyVM();

                if (discrepancy != null)
                {
                    discrepancyVM = ToBusinessObject(discrepancy);
                    discrepancyVM.DiscrepancyHistoryVM = _discrepancyHistoryRepository.List(discrepancyVM.Id);
                }

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DiscrepancyDatatableParams datatableParams)
        {
            try
            {
                List<DiscrepancyDataVM> disperanciesList = _discrepancyRepository.List(datatableParams);

                CreateResponse(disperanciesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private void ManageHistory(Discrepancy oldData, Discrepancy newData)
        {
            DiscrepancyHistory discrepancyHistory = new DiscrepancyHistory();

            discrepancyHistory.DiscrepancyId = newData.Id;
            discrepancyHistory.CreatedBy = newData.CreatedBy;
            discrepancyHistory.CreatedOn = DateTime.UtcNow;

            User userDetails = _userRepository.FindByCondition(p => p.Id == discrepancyHistory.CreatedBy);

            string message = "";

            if (oldData.Id == 0)
            {
                message = $"New discrepancy reported by {userDetails.FirstName} {userDetails.LastName}.";
            }
            else
            {
                if (oldData.DiscrepancyStatusId != newData.DiscrepancyStatusId)
                {
                    List<DataModels.Entities.DiscrepancyStatus> listDiscrepancies = _discrepancyStatusRepository.ListAll();

                    string oldStatus = listDiscrepancies.Find(p => p.Id == oldData.DiscrepancyStatusId).Status;
                    string newStatus = listDiscrepancies.Find(p => p.Id == newData.DiscrepancyStatusId).Status;

                    message = $"Status was changed from {oldStatus} to {newStatus}. ";
                }

                if (oldData.Description != newData.Description)
                {
                    message += $"Description was changed to {newData.Description}. ";
                }

                if (oldData.ActionTaken != newData.ActionTaken)
                {
                    message += $"Action taken was changed to {newData.ActionTaken}. ";
                }
            }

            discrepancyHistory.Message = message;

            if (!string.IsNullOrWhiteSpace(message))
            {
                _discrepancyHistoryRepository.Create(discrepancyHistory);
            }
        }

        #region object mapper

        private DiscrepancyVM ToBusinessObject(Discrepancy discrepancy)
        {
            DiscrepancyVM discrepancyVM = new DiscrepancyVM();

            discrepancyVM.Id = discrepancy.Id;
            discrepancyVM.ReportedByUserId = discrepancy.ReportedByUserId;
            discrepancyVM.AircraftId = discrepancy.AircraftId;
            discrepancyVM.ActionTaken = discrepancy.ActionTaken;
            discrepancyVM.Description = discrepancy.Description;
            discrepancyVM.CompanyId = discrepancy.CompanyId;
            discrepancyVM.DiscrepancyStatusId = discrepancy.DiscrepancyStatusId;
            discrepancyVM.CreatedBy = discrepancy.CreatedBy;
            discrepancyVM.CreatedOn = discrepancy.CreatedOn;

            return discrepancyVM;
        }

        private Discrepancy ToDataObject(DiscrepancyVM discrepancyVM)
        {
            Discrepancy discrepancy = new Discrepancy();

            discrepancy.Id = discrepancyVM.Id;
            discrepancy.ReportedByUserId = discrepancyVM.ReportedByUserId;
            discrepancy.AircraftId = discrepancyVM.AircraftId;
            discrepancy.ActionTaken = discrepancyVM.ActionTaken;
            discrepancy.Description = discrepancyVM.Description;
            discrepancy.CompanyId = discrepancyVM.CompanyId;
            discrepancy.DiscrepancyStatusId = (byte)discrepancyVM.DiscrepancyStatusId;

            if (discrepancyVM.Id == 0)
            {
                discrepancy.CreatedOn = DateTime.UtcNow;
                discrepancy.CreatedBy = discrepancyVM.CreatedBy;
            }
            else
            {
                discrepancy.UpdatedOn = DateTime.UtcNow;
                discrepancy.UpdatedBy = discrepancyVM.UpdatedBy;
            }

            return discrepancy;
        }

        private void SendScheduleMail(DiscrepancyVM discrepancyVM, bool isNew, string timezone)
        {
            try
            {
                if(!isNew && discrepancyVM.DiscrepancyStatusId != (byte)DataModels.Enums.DiscrepancyStatus.Verified_And_Repaired)
                {
                    return;
                }

                Aircraft aircraft = _aircraftRepository.FindByCondition(p => p.Id == discrepancyVM.AircraftId);
                List<User> usersList = _userRepository.ListAllbyCompanyId(discrepancyVM.CompanyId).ToList();
                User reportedBy = _userRepository.FindByCondition(p => p.Id == discrepancyVM.ReportedByUserId);

                DiscrepancyCreatedSendEmailViewModel viewModel = new();
                viewModel.CreatedOn = DateConverter.ToLocal(discrepancyVM.CreatedOn, timezone);
                viewModel.Aircraft = aircraft.TailNo;
                viewModel.ReportedBy = reportedBy.FirstName + " " + reportedBy.LastName;
                
                viewModel.Description = discrepancyVM.Description;
                viewModel.ActionTaken = discrepancyVM.ActionTaken;

                viewModel.Status = _discrepancyStatusRepository.FindByCondition(p => p.Id == discrepancyVM.DiscrepancyStatusId).Status;

                if (isNew)
                {
                    viewModel.Subject = $"{viewModel.Aircraft} - New Discrepancy Added";
                }
                else
                {
                    viewModel.Subject = $"{viewModel.Aircraft} - Discrepancy Closed";
                }

                viewModel.ToEmails.AddRange(usersList.Select(p => p.Email));

                EmailConfiguration emailConfiguration = _emailConfigurationRepository.FindByCondition(p=>p.EmailTypeId == (byte)EmailTypes.Discrepancy);
                if (emailConfiguration != null && !string.IsNullOrWhiteSpace(emailConfiguration.Email))
                {
                    viewModel.CompanyEmail = emailConfiguration.Email;
                }

                _sendMailService.DiscrepancyCreated(viewModel);

                //foreach (User user in usersList)
                //{
                //    viewModel.UserName = user.FirstName + " " + user.LastName;
                //    viewModel.ToEmails = new List<string> { user.Email };
                //    _sendMailService.DiscrepancyCreated(viewModel);
                //}

            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
