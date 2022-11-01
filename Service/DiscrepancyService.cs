using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class DiscrepancyService : BaseService, IDiscrepancyService
    {
        private readonly IDiscrepancyRepository _discrepancyRepository;

        public DiscrepancyService(IDiscrepancyRepository discrepancyRepository)
        {
            _discrepancyRepository = discrepancyRepository;
        }

        public CurrentResponse Create(DiscrepancyVM discrepancyVM)
        {
            Discrepancy discrepancy = ToDataObject(discrepancyVM);

            try
            {
                discrepancy.IsActive = true;
                discrepancy = _discrepancyRepository.Create(discrepancy);
                discrepancyVM = ToBusinessObject(discrepancy);

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "Discrepancy added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(DiscrepancyVM discrepancyVM)
        {
            Discrepancy discrepancy = ToDataObject(discrepancyVM);

            try
            {
                discrepancy = _discrepancyRepository.Edit(discrepancy);
                discrepancyVM = ToBusinessObject(discrepancy);

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "Discrepancy updated successfully");

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
                Discrepancy discrepancy = _discrepancyRepository.FindByCondition(p => p.Id == id);
                DiscrepancyVM discrepancyVM = new DiscrepancyVM();

                if (discrepancy != null)
                {
                    discrepancyVM = ToBusinessObject(discrepancy);
                }

                CreateResponse(discrepancyVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DiscrepancyDatatableParams datatableParams)
        {
            try
            {
                List<DiscrepancyDataVM> disperanciesList = _discrepancyRepository.List(datatableParams);

                CreateResponse(disperanciesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private DiscrepancyVM ToBusinessObject(Discrepancy discrepancy)
        {
            DiscrepancyVM discrepancyVM = new DiscrepancyVM();

            discrepancyVM.Id = discrepancy.Id;
            discrepancyVM.ReportedByUserId = discrepancy.ReportedByUserId;
            discrepancyVM.AircraftId = discrepancy.AircraftId;
            discrepancyVM.FileDisplayName = discrepancy.FileDisplayName;
            discrepancyVM.FileName = discrepancy.FileName;
            discrepancyVM.ActionTaken = discrepancy.ActionTaken;
            discrepancyVM.Description = discrepancy.Description;
            discrepancyVM.CompanyId = discrepancy.CompanyId;
            discrepancyVM.DiscrepancyStatusId = discrepancy.DiscrepancyStatusId;

            return discrepancyVM;
        }

        private Discrepancy ToDataObject(DiscrepancyVM discrepancyVM)
        {
            Discrepancy discrepancy = new Discrepancy();

            discrepancy.Id = discrepancyVM.Id;
            discrepancy.ReportedByUserId = discrepancyVM.ReportedByUserId;
            discrepancy.AircraftId = discrepancyVM.AircraftId;
            discrepancy.FileDisplayName = discrepancyVM.FileDisplayName;
            discrepancy.FileName = discrepancyVM.FileName;
            discrepancy.ActionTaken = discrepancyVM.ActionTaken;
            discrepancy.Description = discrepancyVM.Description;
            discrepancy.CompanyId = discrepancyVM.CompanyId;
            discrepancy.DiscrepancyStatusId = (byte)discrepancyVM.DiscrepancyStatusId;

            if (discrepancyVM.Id == 0)
            {
                discrepancy.CreatedOn = DateTime.UtcNow;
                discrepancy.CreatedBy = discrepancyVM.CreatedBy;
            }
            else
            {
                discrepancy.UpdatedOn = DateTime.UtcNow;
                discrepancy.UpdatedBy = discrepancyVM.UpdatedBy;
            }

            return discrepancy;
        }
    }
}
