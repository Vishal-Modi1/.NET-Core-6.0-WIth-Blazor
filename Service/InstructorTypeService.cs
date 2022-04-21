using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.InstructorType;
using DataModels.VM.Common;

namespace Service
{
    public class InstructorTypeService : BaseService, IInstructorTypeService
    {
        private readonly IInstructorTypeRepository _instructorTypeRepository;

        public InstructorTypeService(IInstructorTypeRepository instructorTypeRepository)
        {
            _instructorTypeRepository = instructorTypeRepository;
        }

        public CurrentResponse Create(InstructorTypeVM instructorTypeVM)
        {
            InstructorType instructorType = new InstructorType() { Name = instructorTypeVM.Name };

            try
            {
                bool isInstructorTypeExist = IsInstructorTypeExist(instructorTypeVM);

                if (isInstructorTypeExist)
                {
                    CreateResponse(instructorType, HttpStatusCode.Ambiguous, "Instructor type is already exist");
                }
                else
                {
                    instructorType = _instructorTypeRepository.Create(instructorType);
                    CreateResponse(instructorType, HttpStatusCode.OK, "Instructor type added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(InstructorTypeVM instructorTypeVM)
        {
            InstructorType instructorType = new InstructorType() { Id = instructorTypeVM.Id, Name = instructorTypeVM.Name };

            try
            {
                bool isInstructorTypeExist = IsInstructorTypeExist(instructorTypeVM);

                if (isInstructorTypeExist)
                {
                    CreateResponse(instructorType, HttpStatusCode.Ambiguous, "Instructor type is already exist");
                }

                else
                {
                    instructorType = _instructorTypeRepository.Edit(instructorType);
                    CreateResponse(instructorType, HttpStatusCode.OK, "Instructor type updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DatatableParams datatableParams)
        {
            try
            {
                List<InstructorTypeVM> instructorTypeList = _instructorTypeRepository.List(datatableParams);

                CreateResponse(instructorTypeList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id, long deletedBy)
        {
            try
            {
                _instructorTypeRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Instructor type deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(int id)
        {
            InstructorType instructorType = _instructorTypeRepository.FindByCondition(p => p.Id == id);
            InstructorTypeVM instructorTypeVM = new InstructorTypeVM();

            if (instructorTypeVM != null)
            {
                instructorTypeVM = ToBusinessObject(instructorType);
            }

            CreateResponse(instructorTypeVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }

        private InstructorTypeVM ToBusinessObject(InstructorType instructorType)
        {
            InstructorTypeVM instructorTypeVM = new InstructorTypeVM();

            instructorTypeVM.Id = instructorType.Id;
            instructorTypeVM.Name = instructorType.Name;

            return instructorTypeVM;
        }

        private bool IsInstructorTypeExist(InstructorTypeVM instructorTypeVM)
        {
            InstructorType instructorType = _instructorTypeRepository.FindByCondition(p => p.Name == instructorTypeVM.Name && p.Id != instructorTypeVM.Id);

            if(instructorType == null)
            {
                return false;
            }

            return true;
        }
    }
}
