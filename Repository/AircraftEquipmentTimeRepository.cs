using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataModels.VM;
using DataModels.VM.AircraftEquipment;

namespace Repository
{
    public class AircraftEquipmentTimeRepository : IAircraftEquipmentTimeRepository
    {
        private MyContext _myContext;
        public AircraftEquipmentTime Create(AircraftEquipmentTime aircraftEquipmentTime)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftEquipmentTimes.Add(aircraftEquipmentTime);
                _myContext.SaveChanges();

                return aircraftEquipmentTime;
            }
        }

        public AircraftEquipmentTime Edit(AircraftEquipmentTime aircraftEquipmentTime)
        {
            using (_myContext = new MyContext())
            {
                AircraftEquipmentTime existingAircraftEquipmentTime = _myContext.AircraftEquipmentTimes.Where(p => p.Id == aircraftEquipmentTime.Id).FirstOrDefault();

                if (existingAircraftEquipmentTime != null)
                {
                    existingAircraftEquipmentTime.EquipmentName = aircraftEquipmentTime.EquipmentName;
                    existingAircraftEquipmentTime.Hours = aircraftEquipmentTime.Hours;
                    existingAircraftEquipmentTime.AircraftId = aircraftEquipmentTime.AircraftId;
                }

                _myContext.SaveChanges();

                return aircraftEquipmentTime;
            }
        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                AircraftEquipmentTime aircraftEquipmentTime = _myContext.AircraftEquipmentTimes.Where(p => p.Id == id).FirstOrDefault();

                if (aircraftEquipmentTime != null)
                {
                    _myContext.AircraftEquipmentTimes.Remove(aircraftEquipmentTime);
                    _myContext.SaveChanges();
                }
            }
        }

        public void DeleteEquipmentTimes(long airCraftId, int UpdatedBy)
        {
            using (_myContext = new MyContext())
            {
                List<AircraftEquipmentTime> aircraftEquipmentTimes = _myContext.AircraftEquipmentTimes.Where(p => p.AircraftId == airCraftId).ToList();

                foreach(var aircraftEquipmentTime in aircraftEquipmentTimes)
                {
                    aircraftEquipmentTime.IsDeleted = true;
                    aircraftEquipmentTime.UpdatedOn = DateTime.UtcNow;
                    aircraftEquipmentTime.UpdatedBy = UpdatedBy;
                    aircraftEquipmentTime.DeletedOn = DateTime.UtcNow;
                    aircraftEquipmentTime.DeletedBy = UpdatedBy;
                }
                _myContext.SaveChanges();
            }
        }

        public AircraftEquipmentTime FindByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipmentTimes.Where(predicate).FirstOrDefault();
            }
        }

        public List<AircraftEquipmentTime> FindListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipmentTimes.Where(predicate).ToList();
            }
        }

        public List<AircraftEquipmentTimeVM> ListByCondition(Expression<Func<AircraftEquipmentTime, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftEquipmentTimes.Where(predicate).ToList().Select(p=> new AircraftEquipmentTimeVM()
                {
                    AircraftId = p.AircraftId,
                    EquipmentName = p.EquipmentName,
                    Hours = p.Hours,
                    Id = p.Id
                    
                }).ToList();
            }
        }
    }
}
