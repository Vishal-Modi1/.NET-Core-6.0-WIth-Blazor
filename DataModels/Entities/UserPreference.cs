namespace DataModels.Entities
{
    public class UserPreference
    {
        public long Id { get; set; }

        public string PreferenceType { get; set; }

        public string PreferencesIds { get; set; }

        public long UserId { get; set; }    
    }
}
