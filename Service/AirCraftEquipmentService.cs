using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.AircraftEquipment;

namespace Service
{
    public class AircraftEquipmentService : BaseService, IAirCraftEquipmentService
    {
        private readonly IAircraftEquipmentRepository _aircraftEquipementRepository;

        public AircraftEquipmentService(IAircraftEquipmentRepository airCraftEquipementRepository)
        {
            _aircraftEquipementRepository = airCraftEquipementRepository;
        }

        public CurrentResponse Create(AircraftEquipmentsVM aircraftEquipmentVM)
        {
            AircraftEquipment aircraftEquipment = ToDataObject(aircraftEquipmentVM);
            try
            {
                aircraftEquipment.IsActive = true;
                aircraftEquipment = _aircraftEquipementRepository.Create(aircraftEquipment);
                CreateResponse(aircraftEquipmentVM, HttpStatusCode.OK, "Aircraft Equipment added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
        
        public CurrentResponse Edit(AircraftEquipmentsVM aircraftEquipmentVM)
        {
            AircraftEquipment aircraftEquipment = ToDataObject(aircraftEquipmentVM);

            try
            {
                aircraftEquipment = _aircraftEquipementRepository.Edit(aircraftEquipment);
                CreateResponse(aircraftEquipment, HttpStatusCode.OK, "Aircraft Equipment updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
        public CurrentResponse Get(int id)
        {
            AircraftEquipment airCraft = _aircraftEquipementRepository.FindByCondition(p => p.Id == id);
            AircraftEquipmentsVM airCraftVM = new AircraftEquipmentsVM();

            if (airCraft != null)
            {
                airCraftVM = ToBusinessObject(airCraft);
            }

            CreateResponse(airCraftVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }
        public CurrentResponse Delete(int id, long deletedBy)
        {
            try
            {
                _aircraftEquipementRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft Equipment is deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(int airCraftId)
        {
            try
            {
                List<AircraftEquipment> airCraft = _aircraftEquipementRepository.FindListByCondition(p => p.AirCraftId == airCraftId  && p.IsDeleted != true);
                List<AircraftEquipmentsVM> airCraftVM = new List<AircraftEquipmentsVM>();

                if (airCraft != null && airCraft.Count() > 0)
                {
                    foreach (var item in airCraft)
                    {
                        airCraftVM.Add(ToBusinessObject(item));
                    }
                }

                CreateResponse(airCraftVM, HttpStatusCode.OK, "");

                return _currentResponse;

                //List<AirCraftEquipment> airCraftEquipment = _aircraftEquipementRepository.List();
                //CreateResponse(airCraftEquipment, HttpStatusCode.OK, "");

                //return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(AircraftEquipmentDatatableParams datatableParams)
        {
            try
            {
                List<AircraftEquipmentDataVM> aircraftEquipmentDataList = _aircraftEquipementRepository.List(datatableParams);

                CreateResponse(aircraftEquipmentDataList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private AircraftEquipment ToDataObject(AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            AircraftEquipment aircraftEquipment = new AircraftEquipment();

            aircraftEquipment.Id = airCraftEquipmentsVM.Id;
            aircraftEquipment.AirCraftId = airCraftEquipmentsVM.AirCraftId;
            aircraftEquipment.StatusId = airCraftEquipmentsVM.StatusId;
            aircraftEquipment.ClassificationId = airCraftEquipmentsVM.ClassificationId;
            aircraftEquipment.AircraftTTInstall = airCraftEquipmentsVM.AircraftTTInstall;
            aircraftEquipment.IsActive = airCraftEquipmentsVM.IsActive;
            aircraftEquipment.IsDeleted = airCraftEquipmentsVM.IsDeleted;
            aircraftEquipment.Item = airCraftEquipmentsVM.Item;
            aircraftEquipment.LogEntryDate = airCraftEquipmentsVM.LogEntryDate;
            aircraftEquipment.Make = airCraftEquipmentsVM.Make;
            aircraftEquipment.Manufacturer = airCraftEquipmentsVM.Manufacturer;
            aircraftEquipment.ManufacturerDate = airCraftEquipmentsVM.ManufacturerDate;
            aircraftEquipment.Model = airCraftEquipmentsVM.Model;
            aircraftEquipment.Notes = airCraftEquipmentsVM.Notes;
            aircraftEquipment.PartNumber = airCraftEquipmentsVM.PartNumber;
            aircraftEquipment.PartTTInstall = airCraftEquipmentsVM.PartTTInstall;
            aircraftEquipment.SerialNumber = airCraftEquipmentsVM.SerialNumber;

            aircraftEquipment.CreatedBy = airCraftEquipmentsVM.CreatedBy;
            if (airCraftEquipmentsVM.Id == 0)
            {
                aircraftEquipment.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                aircraftEquipment.UpdatedBy = airCraftEquipmentsVM.UpdatedBy;
                aircraftEquipment.UpdatedOn = DateTime.UtcNow;
            }

            return aircraftEquipment;
        }
        private AircraftEquipmentsVM ToBusinessObject(AircraftEquipment airCraftEquipment)
        {
            AircraftEquipmentsVM airCraftEquipmentsVM = new AircraftEquipmentsVM();
            airCraftEquipmentsVM.Id = airCraftEquipment.Id;
            airCraftEquipmentsVM.AirCraftId = airCraftEquipment.AirCraftId;
            airCraftEquipmentsVM.StatusId = airCraftEquipment.StatusId;
            airCraftEquipmentsVM.ClassificationId = airCraftEquipment.ClassificationId;
            airCraftEquipmentsVM.AircraftTTInstall = airCraftEquipment.AircraftTTInstall;
            airCraftEquipmentsVM.Item = airCraftEquipment.Item;
            airCraftEquipmentsVM.LogEntryDate = airCraftEquipment.LogEntryDate;
            airCraftEquipmentsVM.Make = airCraftEquipment.Make;
            airCraftEquipmentsVM.Manufacturer = airCraftEquipment.Manufacturer;
            airCraftEquipmentsVM.ManufacturerDate = airCraftEquipment.ManufacturerDate;
            airCraftEquipmentsVM.Model = airCraftEquipment.Model;
            airCraftEquipmentsVM.Notes = airCraftEquipment.Notes;
            airCraftEquipmentsVM.PartNumber = airCraftEquipment.PartNumber;
            airCraftEquipmentsVM.PartTTInstall = airCraftEquipment.PartTTInstall;
            airCraftEquipmentsVM.SerialNumber = airCraftEquipment.SerialNumber;

            airCraftEquipmentsVM.IsActive = airCraftEquipment.IsActive;
            airCraftEquipmentsVM.IsDeleted = airCraftEquipment.IsDeleted;
            airCraftEquipmentsVM.CreatedBy = airCraftEquipment.CreatedBy;
            airCraftEquipmentsVM.UpdatedBy = airCraftEquipment.UpdatedBy;
            airCraftEquipmentsVM.DeletedBy = airCraftEquipment.DeletedBy;
            airCraftEquipmentsVM.CreatedOn = airCraftEquipment.CreatedOn;
            airCraftEquipmentsVM.UpdatedOn = airCraftEquipment.UpdatedOn;
            airCraftEquipmentsVM.DeletedOn = airCraftEquipment.DeletedOn;

            return airCraftEquipmentsVM;
        }

       
    }
}
