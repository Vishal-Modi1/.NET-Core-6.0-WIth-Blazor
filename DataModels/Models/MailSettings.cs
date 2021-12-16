namespace DataModels.Models
{
    public class MailSettings
    {
        public string Host { get; set; }
        public int Port{ get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromName { get; set; }
        public string CC { get; set; }
        public bool IsBodyHTML { get; set; }
        public bool IsEnableSSL { get; set; }
    }
}
