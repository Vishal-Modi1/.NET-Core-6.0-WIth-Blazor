using System;

namespace DataModels.Entities
{
    public class EmailToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ExpireOn { get; set; }
        public int? UserId { get; set; }
        public string EmailType { get; set; }
    }
}
