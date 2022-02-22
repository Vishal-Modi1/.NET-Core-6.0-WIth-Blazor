using DataModels.Enums;
using System.Collections.Generic;

namespace DataModels.VM.UserPreference
{
    public class UserPreferenceVM
    {
        public long Id { get; set; }

        public PreferenceType PreferenceType { get; set; }

        public List<string> ListPreferencesIds { get; set; }

        public long UserId { get; set; }
    }
}
