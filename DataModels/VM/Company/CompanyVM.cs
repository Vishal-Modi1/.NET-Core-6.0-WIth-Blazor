using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.Company
{
    public class CompanyVM : CommonField
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        [RegularExpression("\\b\\d{5}\\b", ErrorMessage = "Invalid zipcode")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Timezone is required")]
        public string TimeZone { get; set; }

        //[Url(ErrorMessage = "Please enter valid url")]
        [RegularExpression(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", ErrorMessage = "Please enter valid url")]
        public string Website { get; set; }
        public string PrimaryAirport { get; set; }
        public short? PrimaryServiceId { get; set; }
        public List<DropDownValues> PrimaryServicesList { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter 10 digits no")]
        public string ContactNo { get; set; }
        public string Logo { get; set; }

        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
        [NotMapped]
        public string LogoPath { get; set; }

        public int TotalAircrafts { get; set; }

        public int TotalUsers { get; set; }
        [NotMapped]
        public new long? CreatedBy { get; set; }
        public int TotalRecords { get; set; }
    }
}
