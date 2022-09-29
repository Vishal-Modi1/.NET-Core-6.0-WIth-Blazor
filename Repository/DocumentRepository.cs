using DataModels.Entities;
using DataModels.VM.Document;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private MyContext _myContext;

        public Document Create(Document document)
        {
            using (_myContext = new MyContext())
            {
                document.Id = Guid.NewGuid();

                _myContext.Documents.Add(document);
                _myContext.SaveChanges();

                return document;
            }
        }

        public Document Edit(Document document)
        {
            using (_myContext = new MyContext())
            {
                Document existingDocument = _myContext.Documents.Where(p=>p.Id == document.Id).FirstOrDefault();

                if(existingDocument == null)
                {
                    return document;
                }

                existingDocument.Name = document.Name;
                existingDocument.DisplayName = document.DisplayName;
                existingDocument.ExpirationDate = document.ExpirationDate;
                existingDocument.UpdatedOn = document.UpdatedOn;
                existingDocument.UpdatedBy = document.UpdatedBy;
                existingDocument.Type = document.Type;
                existingDocument.ModuleId = document.ModuleId;
                existingDocument.CompanyId = document.CompanyId;
                existingDocument.UserId = document.UserId;
                existingDocument.TagIds = document.TagIds;
                existingDocument.LastShareDate = document.LastShareDate;
                existingDocument.IsShareable = document.IsShareable;

                _myContext.SaveChanges();

                return document;
            }
        }

        public bool UpdateDocumentName(Guid id, string name)
        {
            using (_myContext = new MyContext())
            {
                Document existingDocument = _myContext.Documents.Where(p => p.Id == id).FirstOrDefault();

                if (existingDocument != null)
                {
                    existingDocument.Name = name;
                    _myContext.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public Document FindByCondition(Expression<Func<Document, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                Document existingDocument = _myContext.Documents.Where(predicate).FirstOrDefault();
                
                return existingDocument;
            }
        }

        public List<DocumentDataVM> List(DocumentDatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<DocumentDataVM> list;

                string aircraftId = "";

                if(datatableParams.AircraftId != null)
                {
                    aircraftId = $",{datatableParams.AircraftId.ToString()}";
                }

                string sql = $"EXEC dbo.GetDocumentList '{ datatableParams.SearchText }', { datatableParams.Start }, " +
                    $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', " +
                    $"{datatableParams.IsPersonalDocument},{datatableParams.CompanyId},{datatableParams.ModuleId}," +
                    $"{datatableParams.UserId}{aircraftId}";

                list = _myContext.DocumentDataVM.FromSqlRaw<DocumentDataVM>(sql).ToList();

                return list;
            }
        }

        public void Delete(Guid id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                Document document = _myContext.Documents.Where(p => p.Id == id).FirstOrDefault();

                if (document != null)
                {
                    document.IsDeleted = true;
                    document.DeletedBy = deletedBy;
                    document.DeletedOn = DateTime.UtcNow;

                    _myContext.SaveChanges();
                }
            }
        }

        public long UpdateTotalDownloads(Guid id)
        {
            using (_myContext = new MyContext())
            {
                Document document = _myContext.Documents.Where(p => p.Id == id).FirstOrDefault();

                if (document != null)
                {
                    document.TotalDownloads = document.TotalDownloads.GetValueOrDefault() + 1;
                    _myContext.SaveChanges();

                    return Convert.ToInt64(document.TotalDownloads);
                }

                return 0;
            }
        }

        public long UpdateTotalShares(Guid id)
        {
            using (_myContext = new MyContext())
            {
                Document document = _myContext.Documents.Where(p => p.Id == id).FirstOrDefault();

                if (document != null)
                {
                    document.TotalShares = document.TotalShares.GetValueOrDefault() + 1;
                    _myContext.SaveChanges();

                    return Convert.ToInt64(document.TotalShares);
                }

                return 0;
            }
        }
    }
}
