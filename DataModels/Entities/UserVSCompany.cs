namespace DataModels.Entities
{
    public class UserVSCompany : CommonField
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int? CompanyId { get; set; }
        public int RoleId { get; set; }
    }
}
