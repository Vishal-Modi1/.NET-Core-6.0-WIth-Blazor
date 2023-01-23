using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class LogBookRepository : BaseRepository<LogBook>, ILogBookRepository
    {
        private readonly MyContext _myContext;

        public LogBookRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void Create(LogBookVM logBookVM)
        {

        }

        public List<DropDownSmallValues> ListInstrumentApproachesDropdownValues()
        {
            List<DropDownSmallValues> approaches = (from approach in _myContext.InstrumentApproaches
                                                     select new DropDownSmallValues()
                                                     {
                                                         Id = approach.Id,
                                                         Name = approach.Name
                                                     }).ToList();

            return approaches;
        }
    }
}
