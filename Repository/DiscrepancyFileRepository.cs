using DataModels.Entities;
using DataModels.VM.Discrepancy;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class DiscrepancyFileRepository : BaseRepository<DiscrepancyFile>, IDiscrepancyFileRepository
    {
        private readonly MyContext _myContext;

        public DiscrepancyFileRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public DiscrepancyFile Edit(DiscrepancyFile discrepancyFile)
        {
            DiscrepancyFile existingDiscrepancyFile = _myContext.DiscrepancyFiles.Where(p => p.Id == discrepancyFile.Id).FirstOrDefault();

            if (existingDiscrepancyFile != null)
            {
                existingDiscrepancyFile.Name = discrepancyFile.Name;
                existingDiscrepancyFile.DisplayName = discrepancyFile.DisplayName;
                _myContext.SaveChanges();

                return existingDiscrepancyFile;
            }

            return discrepancyFile;
        }

        public List<DiscrepancyFileVM> List(long discrepancyId)
        {
            List<DiscrepancyFileVM> listData = (from fileData in _myContext.DiscrepancyFiles
                                                join user in _myContext.Users
                                                on fileData.CreatedBy equals user.Id
                                                where fileData.DiscrepancyId == discrepancyId
                                                && fileData.IsActive == true && fileData.IsDeleted == false
                                                select new DiscrepancyFileVM()
                                                {
                                                    Id = fileData.Id,
                                                    DiscrepancyId = fileData.DiscrepancyId,
                                                    DisplayName = fileData.DisplayName,
                                                    CreatedOn = fileData.CreatedOn,
                                                    AddedBy = user.FirstName + " " + user.LastName,
                                                    Name = fileData.Name
                                                }).ToList();

            return listData;
        }

        public bool UpdateDocumentName(long id, string name)
        {
            DiscrepancyFile existingFile = _myContext.DiscrepancyFiles.Where(p => p.Id == id).FirstOrDefault();

            if (existingFile != null)
            {
                existingFile.Name = name;
                _myContext.SaveChanges();

                return true;
            }

            return false;
        }

        public void Delete(long id, long deletedBy)
        {
            DiscrepancyFile discrepancyFile = _myContext.DiscrepancyFiles.Where(p => p.Id == id).FirstOrDefault();

            if (discrepancyFile != null)
            {
                discrepancyFile.IsDeleted = true;
                discrepancyFile.DeletedBy = deletedBy;
                discrepancyFile.DeletedOn = DateTime.UtcNow;

                _myContext.SaveChanges();
            }
        }
    }
}
