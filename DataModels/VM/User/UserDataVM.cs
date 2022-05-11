namespace DataModels.VM.User
{
    public class UserDataVM
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }
        public int? CompanyId { get; set; }

        public string ProfileImage { get; set; }

        public string Email { get; set; }

        public bool IsInstructor { get; set; }
        
        public bool IsActive { get; set; }

        public string UserRole { get; set; }

        public int TotalRecords { get; set; }
    }
}
