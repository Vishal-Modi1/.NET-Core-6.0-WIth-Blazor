using DataModels.CustomValidations;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Document
{
    public class DocumentTagVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string TagName { get; set; }

        public List<DropDownValues> CompniesList { get; set; }

        [RequiredIf(nameof(IsGlobalTag), false,ErrorMessage = "Company is required")]
        public int?  CompanyId { get; set; }

        public bool IsGlobalTag { get; set; }

        public long CreatedBy { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
