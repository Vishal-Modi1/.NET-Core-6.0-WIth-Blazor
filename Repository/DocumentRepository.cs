using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Document;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GlobalUtilities.Extensions;

namespace Repository
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;

        public DocumentRepository(MyContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            this._myContext = dbContext;
            _mapper = mapper;
        }

        public override Document Create(Document document)
        {
            document.Id = Guid.NewGuid();

            _myContext.Documents.Add(document);
            _myContext.SaveChanges();

            return document;
        }

        public Document Edit(Document document)
        {
            Document existingDocument = _myContext.Documents.Where(p => p.Id == document.Id).FirstOrDefault();

            if (existingDocument == null)
            {
                return document;
            }

            _mapper.Map(document, existingDocument);
            _myContext.SaveChanges();

            return document;
        }

        public bool UpdateDocumentName(Guid id, string name)
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

        public List<DocumentDataVM> List(DocumentDatatableParams datatableParams)
        {
            List<DocumentDataVM> list;

            var param = new SqlParameter[] {
                        new SqlParameter() { ParameterName = "@RoleId", Value = (short)datatableParams.UserRole},
                        new SqlParameter() {ParameterName = "@SearchValue",Value = datatableParams.SearchText.EmptyStringIfNull()},
                        new SqlParameter() {ParameterName = "@PageNo",Value = datatableParams.Start},
                        new SqlParameter() {ParameterName = "@PageSize",Value = datatableParams.Length},
                        new SqlParameter() {ParameterName = "@SortColumn",Value = datatableParams.SortOrderColumn},
                        new SqlParameter() {ParameterName = "@SortOrder",Value = datatableParams.OrderType},
                        new SqlParameter() {ParameterName = "@IsPersonalDocument",Value = datatableParams.IsFromMyProfile},
                        new SqlParameter() {ParameterName = "@CompanyId",Value = datatableParams.CompanyId},
                        new SqlParameter() {ParameterName = "@ModuleId",Value = datatableParams.ModuleId},
                        new SqlParameter() {ParameterName = "@UserId",Value = datatableParams.UserId},
                        new SqlParameter() {ParameterName = "@AircraftId",Value = datatableParams.AircraftId == null ? 0: datatableParams.AircraftId},
                        new SqlParameter() {ParameterName = "@DocumentType",Value = datatableParams.DocumentType.EmptyStringIfNull()},
                        new SqlParameter() {ParameterName = "@DocumentDirectoryId",Value = datatableParams.DocumentDirectoryId == null ? DBNull.Value : datatableParams.DocumentDirectoryId},
            };

            string sql = "[dbo].[GetDocumentsList] @RoleId, @SearchValue,@PageNo,@PageSize,@SortColumn, @SortOrder, @IsPersonalDocument, @CompanyId, @ModuleId, @UserId, @AircraftId, @DocumentType, @DocumentDirectoryId";

            list = _myContext.DocumentDataVM.FromSqlRaw(sql,param).ToList();

            return list;
        }

        public void Delete(Guid id, long deletedBy)
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

        public long UpdateTotalDownloads(Guid id)
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

        public long UpdateTotalShares(Guid id)
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
