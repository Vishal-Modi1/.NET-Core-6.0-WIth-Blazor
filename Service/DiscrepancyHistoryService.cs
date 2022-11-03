using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;

namespace Service
{
    public class DiscrepancyHistoryService : BaseService, IDiscrepancyHistoryService
    {
        private readonly IDiscrepancyHistoryRepository _discrepancyHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDiscrepancyStatusRepository _discrepancyStatusRepository;

        public DiscrepancyHistoryService(IDiscrepancyHistoryRepository discrepancyHistoryRepository,
          IUserRepository userRepository, IDiscrepancyStatusRepository discrepancyStatusRepository)
        {
            _discrepancyHistoryRepository = discrepancyHistoryRepository;
            _userRepository = userRepository;
            _discrepancyStatusRepository = discrepancyStatusRepository;
        }

        public void Create(Discrepancy oldData, Discrepancy newData)
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
                    List<DiscrepancyStatus> listDiscrepancies = _discrepancyStatusRepository.ListAll();

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
    }
}
