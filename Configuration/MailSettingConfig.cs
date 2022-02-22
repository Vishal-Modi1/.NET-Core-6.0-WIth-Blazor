using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Configuration
{
    public class MailSettingConfig
    {
        private static MailSettingConfig _instance = null;
        private static readonly object padlock = new object();
        private static IConfiguration configuration;

        #region Object Creation
        private MailSettingConfig()
        {
            configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("configuration.json", optional: true, reloadOnChange: true)
                    .Build().GetSection("MailSettings");
        }

        public static MailSettingConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new MailSettingConfig();
                    }
                    return _instance;
                }
            }
        }

        #endregion
 
        public string Host
        {
            get => configuration.GetValue<string>("Host");
        }
        public int Port
        {
            get => Convert.ToInt32(configuration.GetValue<string>("Port"));
        }
        public string FromEmail
        {
            get => configuration.GetValue<string>("FromEmail");
        }
        public string Password
        {
            get => configuration.GetValue<string>("Password");
        }
        public bool EnabelSSL
        {
            get => Convert.ToBoolean(configuration.GetValue<string>("EnabelSSL"));
        }
    }
}
