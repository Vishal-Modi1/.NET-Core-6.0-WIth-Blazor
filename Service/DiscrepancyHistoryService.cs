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

        //public void Create(Discrepancy oldData, Discrepancy newData)
        //{
            
        //}
    }
}
