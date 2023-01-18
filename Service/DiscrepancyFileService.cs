using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace Service
{
    public class DiscrepancyFileService : BaseService, IDiscrepancyFileService
    {
        private readonly IDiscrepancyFileRepository _discrepancyFileRepository;
        private readonly IDiscrepancyRepository _discrepancyRepository;
        private readonly IDiscrepancyHistoryRepository _discrepancyHistoryRepository;
        private readonly IDiscrepancyStatusRepository _discrepancyStatusRepository;
        private readonly IUserRepository _userRepository;

        public DiscrepancyFileService(IDiscrepancyFileRepository discrepancyFileRepository, 
            IDiscrepancyRepository discrepancyRepository,IUserRepository userRepository,
            IDiscrepancyHistoryRepository discrepancyHistoryRepository,
            IDiscrepancyStatusRepository discrepancyStatusRepository)
        {
            _discrepancyFileRepository = discrepancyFileRepository;
            _discrepancyRepository = discrepancyRepository; _discrepancyHistoryRepository = discrepancyHistoryRepository;
            _discrepancyStatusRepository = discrepancyStatusRepository;
            _userRepository = userRepository;
        }

        public CurrentResponse Create(DiscrepancyFileVM discrepancyFileVM)
        {
            try
            {
                DiscrepancyFile discrepancyFile = ToDataObject(discrepancyFileVM);
                discrepancyFile = _discrepancyFileRepository.Create(discrepancyFile);
                ManageHistory(new DiscrepancyFile(), discrepancyFile);

                CreateResponse(discrepancyFile, HttpStatusCode.OK, "File added successfully.");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(DiscrepancyFileVM discrepancyFileVM)
        {
            try
            {
                DiscrepancyFile discrepancyFile = ToDataObject(discrepancyFileVM);
                DiscrepancyFile oldDiscrepancyFile = _discrepancyFileRepository.FindByCondition(p => p.Id == discrepancyFileVM.Id);
                discrepancyFile = _discrepancyFileRepository.Edit(discrepancyFile);

                ManageHistory(oldDiscrepancyFile, discrepancyFile);

                CreateResponse(discrepancyFile, HttpStatusCode.OK, "File updated successfully.");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateDocumentName(long id, string name)
        {
            try
            {
                bool isImageNameUpdated = _discrepancyFileRepository.UpdateDocumentName(id, name);
                DiscrepancyFile discrepancyFile = _discrepancyFileRepository.FindByCondition(p => p.Id == id);

                CreateResponse(discrepancyFile, HttpStatusCode.OK, "Document updated successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindByCondition(Expression<Func<DiscrepancyFile, bool>> predicate)
        {
            try
            {
                DiscrepancyFile discrepancyFile = _discrepancyFileRepository.FindByCondition(predicate);
                CreateResponse(discrepancyFile, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(long discrepancyId)
        {
            try
            {
                List<DiscrepancyFileVM> disperancyFilesList = _discrepancyFileRepository.List(discrepancyId);

                if (disperancyFilesList.Count > 0)
                {
                    Discrepancy discrepancy = _discrepancyRepository.FindByCondition(p => p.Id == disperancyFilesList.First().DiscrepancyId);

                    foreach (DiscrepancyFileVM item in disperancyFilesList)
                    {
                        item.FilePath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}{UploadDirectories.Discrepancy}/{discrepancy.CompanyId}/{discrepancy.AircraftId}/{item.Name}";
                    }
                }

                CreateResponse(disperancyFilesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(long id)
        {
            try
            {
                DiscrepancyFile discrepancyFile = _discrepancyFileRepository.FindByCondition(p => p.Id == id);
                DiscrepancyFileVM discrepancyFileVM = new DiscrepancyFileVM();

                if (discrepancyFile != null)
                {
                    discrepancyFileVM = ToBusinessObject(discrepancyFile);
                    
                    Discrepancy discrepancy = _discrepancyRepository.FindByCondition(p=>p.Id == discrepancyFileVM.DiscrepancyId);
                    discrepancyFileVM.FilePath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.Discrepancy}/{discrepancy.CompanyId}/{discrepancy.AircraftId}/{discrepancyFileVM.Name}";
                }

                CreateResponse(discrepancyFileVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _discrepancyFileRepository.Delete(id, deletedBy);
                var data = _discrepancyFileRepository.FindByCondition(p=>p.Id == id);
                User userDetails = _userRepository.FindByCondition(p => p.Id == deletedBy);
                string message = $"File has been deleted by {userDetails.FirstName} {userDetails.LastName}.";

                CreateHistory(message, deletedBy, data.DiscrepancyId);

                CreateResponse(true, HttpStatusCode.OK, "File deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private void ManageHistory(DiscrepancyFile oldData, DiscrepancyFile newData)
        {
            User userDetails = _userRepository.FindByCondition(p => p.Id == newData.CreatedBy);

            string message = "";

            if (oldData.Id == 0)
            {
                message = $"New file added by {userDetails.FirstName} {userDetails.LastName}.";
            }
            else
            {
                if (oldData.DisplayName != newData.DisplayName)
                {
                    message += $"File name was changed from {oldData.DisplayName} to {newData.DisplayName}. ";
                }
            }

            CreateHistory(message, newData.CreatedBy, newData.DiscrepancyId);
        }

        private void CreateHistory(string message, long createdBy, long discrepancyId)
        {
            DiscrepancyHistory discrepancyHistory = new DiscrepancyHistory();

            discrepancyHistory.DiscrepancyId = discrepancyId;
            discrepancyHistory.CreatedBy = createdBy;
            discrepancyHistory.CreatedOn = DateTime.UtcNow;
            discrepancyHistory.Message = message;

            if (!string.IsNullOrWhiteSpace(discrepancyHistory.Message))
            {
                _discrepancyHistoryRepository.Create(discrepancyHistory);
            }
        }

        private DiscrepancyFileVM ToBusinessObject(DiscrepancyFile discrepancyFile)
        {
            DiscrepancyFileVM discrepancyFileVM = new DiscrepancyFileVM();

            discrepancyFileVM.Id = discrepancyFile.Id;
            discrepancyFileVM.DiscrepancyId = discrepancyFile.DiscrepancyId;
            discrepancyFileVM.DisplayName = discrepancyFile.DisplayName;
            discrepancyFileVM.Name = discrepancyFile.Name;
            discrepancyFileVM.CreatedBy = discrepancyFile.CreatedBy;
            discrepancyFileVM.CreatedOn = discrepancyFile.CreatedOn;

            return discrepancyFileVM;
        }

        private DiscrepancyFile ToDataObject(DiscrepancyFileVM discrepancyFileVM)
        {
            DiscrepancyFile discrepancyFile = new DiscrepancyFile();

            discrepancyFile.Id = discrepancyFileVM.Id;
            discrepancyFile.DiscrepancyId = discrepancyFileVM.DiscrepancyId;
            discrepancyFile.DisplayName = discrepancyFileVM.DisplayName;
            discrepancyFile.Name = discrepancyFileVM.Name;
            discrepancyFile.IsActive = true;

            if (discrepancyFileVM.Id == 0)
            {
                discrepancyFile.CreatedOn = DateTime.UtcNow;
                discrepancyFile.CreatedBy = discrepancyFileVM.CreatedBy;
            }
            else
            {
                discrepancyFile.UpdatedOn = DateTime.UtcNow;
                discrepancyFile.UpdatedBy = discrepancyFileVM.UpdatedBy;
            }

            return discrepancyFile;
        }
    }
}
