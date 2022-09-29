using DataModels.CustomValidations;
using DataModels.Enums;
using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.UserPreference
{
    public class UserPreferenceVM
    {
        public long Id { get; set; }

        public PreferenceType PreferenceType { get; set; }

        public List<string> ListPreferencesIds { get; set; }

        [Range(1, int.MaxValue ,ErrorMessage = "Select preference type")]
        public int PreferenceTypeId { get; set; }

        public List<DropDownValues> PreferenceTypesList { get; set; }
        public List<DropDownLargeValues> AircraftList { get; set; }
        public List<DropDownLargeValues> ActivityTypeList { get; set; }

        [Required, MinLength(1, ErrorMessage = "At least one aircraft required")]
        public List<long> AircraftIds { get; set; }

        [Required, MinLength(1, ErrorMessage = "At least one activity required")]
        public List<long> ActivityIds { get; set; }
        public long UserId { get; set; }
    }
}
