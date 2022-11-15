using DataModels.Entities;
using DataModels.VM.AircraftModel;
using DataModels.VM.Common;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class AircraftModelRepository : IAircraftModelRepository
    {
        private MyContext _myContext;

        public List<AircraftModel> List()
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftModels.ToList();
            }
        }

        public AircraftModel Create(AircraftModel aircraftModel)
        {
            using (_myContext = new MyContext())
            {
                _myContext.AircraftModels.Add(aircraftModel);
                _myContext.SaveChanges();

                return aircraftModel;
            }
        }

        public AircraftModel FindByCondition(Expression<Func<AircraftModel, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.AircraftModels.Where(predicate).FirstOrDefault();
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> aircraftModelList = (from aircraftModel in _myContext.AircraftModels
                                                         select new DropDownValues()
                                                         {
                                                             Id = aircraftModel.Id,
                                                             Name = aircraftModel.Name

                                                         }).ToList();

                return aircraftModelList;
            }
        }

        public List<AircraftModelDataVM> List(DatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<AircraftModelDataVM> list;

                string sql = $"EXEC dbo.GetAircraftModelsList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                    $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                list = _myContext.AircraftModelsList.FromSqlRaw<AircraftModelDataVM>(sql).ToList();

                return list;

            }
        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                AircraftModel aircraftModel = _myContext.AircraftModels.Where(p => p.Id == id).FirstOrDefault();

                if (aircraftModel != null)
                {
                    _myContext.AircraftModels.Remove(aircraftModel);
                    _myContext.SaveChanges();
                }
            }
        }

        public bool IsAlreadyUsed(int id)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Aircrafts.Where(p => p.AircraftModelId == id).Any();
            }
        }

        public AircraftModel Edit(AircraftModel aircraftModel)
        {
            using (_myContext = new MyContext())
            {
                AircraftModel existingAircraftModel = _myContext.AircraftModels.Where(p => p.Id == aircraftModel.Id).FirstOrDefault();

                if (existingAircraftModel != null)
                {
                    existingAircraftModel.Name = aircraftModel.Name;
                    _myContext.SaveChanges();
                }

                return aircraftModel;
            }
        }
    }
}
