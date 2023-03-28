using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Document.DocumentDirectory;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class DocumentDirectoryRepository : BaseRepository<DocumentDirectory>, IDocumentDirectoryRepository
    {
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;

        public DocumentDirectoryRepository(MyContext dbContext, IMapper mappper)
            : base(dbContext)
        {
            this._myContext = dbContext;
            this._mapper = mappper;
        }

        public DocumentDirectory Edit(DocumentDirectory documentDirectory)
        {
            DocumentDirectory existingDocumentDirectory = _myContext.DocumentDirectories.Where(p => p.Id == documentDirectory.Id).FirstOrDefault();

            if(existingDocumentDirectory == null)
            {
                return documentDirectory;
            }

            existingDocumentDirectory = _mapper.Map<DocumentDirectory>(documentDirectory);

            //existingDocumentDirectory.Name = documentDirectory.Name;
            //existingDocumentDirectory.CompanyId = documentDirectory.CompanyId;
            //existingDocumentDirectory.UpdatedBy = documentDirectory.UpdatedBy;
            //existingDocumentDirectory.UpdatedOn = documentDirectory.UpdatedOn;

            _myContext.SaveChanges();

            return documentDirectory;
        }

        public List<DocumentDirectorySummaryVM> ListWithCountByComapnyId(int companyId)
        {
            List<DocumentDirectorySummaryVM> list;

            string sql = $"EXEC dbo.GetDocumentDirectoriesWithCountByCompanyId {companyId}";
            list = _myContext.DocumentDirectorySummaryVM.FromSqlRaw(sql).ToList();

            return list;
        }

        public void Delete(long id, long deletedBy)
        {
            DocumentDirectory documentDirectory = _myContext.DocumentDirectories.Where(p => p.Id == id).FirstOrDefault();

            if (documentDirectory != null)
            {
                documentDirectory.IsDeleted = true;
                documentDirectory.DeletedBy = deletedBy;
                documentDirectory.DeletedOn = DateTime.UtcNow;

                _myContext.SaveChanges();
            }
        }

        public List<DropDownLargeValues> ListDropDownValuesByCompanyId(int companyId)
        {
            List<DropDownLargeValues> documentDirectories = _myContext.DocumentDirectories.Where
                                                            (p => p.IsActive && !p.IsDeleted && p.CompanyId == companyId).
                                                            Select(p => new DropDownLargeValues()
                                                            {
                                                                Id = p.Id,
                                                                Name = p.Name,
                                                            }).ToList();

            return documentDirectories;
        }
    }
}
