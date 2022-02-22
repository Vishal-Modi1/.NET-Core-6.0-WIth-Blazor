using System;

namespace DataModels.Entities
{
    public class EmailToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ExpireOn { get; set; }
        public long? UserId { get; set; }
        public string EmailType { get; set; }
    }
}
