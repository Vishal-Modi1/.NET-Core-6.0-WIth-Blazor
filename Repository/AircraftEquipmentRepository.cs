using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataModels.VM.AircraftEquipment;

namespace Repository
{
    public class AirCraftEquipmentRepository : IAircraftEquipmentRepository
    {
        private MyContext _myContext;

        public AircraftEquipment Create(AircraftEquipment aircraftEquipment)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftEquipments.Add(aircraftEquipment);
                _myContext.SaveChanges();

                return aircraftEquipment;
            }
        }

        public AircraftEquipment Edit(AircraftEquipment aircraftEquipment)
        {
            using (_myContext = new MyContext())
            {
                AircraftEquipment existingAircraftEquipment = _myContext.AircraftEquipments.Where(p => p.Id == aircraftEquipment.Id).FirstOrDefault();

                if (existingAircraftEquipment != null)
                {
                    existingAircraftEquipment.Id = aircraftEquipment.Id;
                    existingAircraftEquipment.StatusId = aircraftEquipment.StatusId;
                    existingAircraftEquipment.ClassificationId = aircraftEquipment.ClassificationId;
                    existingAircraftEquipment.AircraftTTInstall = aircraftEquipment.AircraftTTInstall;
                    existingAircraftEquipment.Item = aircraftEquipment.Item;
                    existingAircraftEquipment.LogEntryDate = aircraftEquipment.LogEntryDate;
                    existingAircraftEquipment.Make = aircraftEquipment.Make;
                    existingAircraftEquipment.Manufacturer = aircraftEquipment.Manufacturer;
                    existingAircraftEquipment.ManufacturerDate = aircraftEquipment.ManufacturerDate;
                    existingAircraftEquipment.Model = aircraftEquipment.Model;
                    existingAircraftEquipment.Notes = aircraftEquipment.Notes;
                    existingAircraftEquipment.PartNumber = aircraftEquipment.PartNumber;
                    existingAircraftEquipment.PartTTInstall = aircraftEquipment.PartTTInstall;
                    existingAircraftEquipment.SerialNumber = aircraftEquipment.SerialNumber;
                    existingAircraftEquipment.IsActive = aircraftEquipment.IsActive;
                    existingAircraftEquipment.IsDeleted = aircraftEquipment.IsDeleted;
                    existingAircraftEquipment.UpdatedBy = aircraftEquipment.UpdatedBy;
                    existingAircraftEquipment.UpdatedOn = aircraftEquipment.UpdatedOn;
                }

                _myContext.SaveChanges();

                return aircraftEquipment;
            }
        }

        public void Delete(int id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                AircraftEquipment aircraftEquipment = _myContext.AircraftEquipments.Where(p => p.Id == id).FirstOrDefault();

                if (aircraftEquipment != null)
                {
                    aircraftEquipment.IsDeleted = true;
                    aircraftEquipment.DeletedOn = DateTime.UtcNow;
                    aircraftEquipment.DeletedBy = deletedBy;

                    _myContext.SaveChanges();
                }
            }
        }

        public List<AircraftEquipment> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipments.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            }
        }

        public List<AircraftEquipmentDataVM> List(AircraftEquipmentDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<AircraftEquipmentDataVM> list;

                string sql = $"EXEC dbo.GetAircraftEquipmentList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', { datatableParams.AircraftId }";

                list = _myContext.AircraftEquipmentData.FromSqlRaw<AircraftEquipmentDataVM>(sql).ToList();

                return list;
            }
        }

        public AircraftEquipment FindByCondition(Expression<Func<AircraftEquipment, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipments.Where(predicate).FirstOrDefault();
            }
        }

        public List<AircraftEquipment> FindListByCondition(Expression<Func<AircraftEquipment, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipments.Where(predicate).ToList();
            }
        }
    }
}
