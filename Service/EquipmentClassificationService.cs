using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;

namespace Service
{
    public class EquipmentClassificationService : BaseService, IEquipmentClassificationService
    {
        private readonly IEquipmentClassificationRepository _equipmentClassificationRepository;

        public EquipmentClassificationService(IEquipmentClassificationRepository equipmentClassificationRepository)
        {
            _equipmentClassificationRepository = equipmentClassificationRepository;
        }

        public CurrentResponse List()
        {
            try
            {
                List<EquipmentClassification> statuses = new List<EquipmentClassification>();
                statuses = _equipmentClassificationRepository.List();
                CreateResponse(statuses, HttpStatusCode.OK, "");
                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.BadRequest, exc.ToString());
                return _currentResponse;
            }
        }
    }
}
