using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;

namespace Service
{
    public class StatusService : BaseService, IEquipmentStatusService
    {
        private readonly IEquipmentStatusRepository _equipmentStatusRepository;

        public StatusService(IEquipmentStatusRepository equipmentStatusRepository)
        {
            _equipmentStatusRepository = equipmentStatusRepository;
        }
        public CurrentResponse List()
        {
            try
            {
                List<EquipmentStatus> equipmentStatus = new List<EquipmentStatus>();
                equipmentStatus = _equipmentStatusRepository.List();
                CreateResponse(equipmentStatus, HttpStatusCode.OK, "");
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
