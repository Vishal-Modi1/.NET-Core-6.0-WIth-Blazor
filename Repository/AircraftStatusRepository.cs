using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AircraftStatusRepository : BaseRepository<AircraftStatus>, IAircraftStatusRepository
    {
        private readonly MyContext _myContext;

        public AircraftStatusRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<DropDownValues> ListDropDownValues()
        {
            List<DropDownValues> aircraftStatusList = (from aircraftStatus in _myContext.AircraftStatuses
                                                       select new DropDownValues()
                                                       {
                                                           Id = aircraftStatus.Id,
                                                           Name = aircraftStatus.Status
                                                       }).ToList();

            return aircraftStatusList;
        }
    }
}
