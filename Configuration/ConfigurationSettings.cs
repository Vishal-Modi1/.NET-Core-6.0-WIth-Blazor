using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Configuration
{
    public class ConfigurationSettings
    {
        private static ConfigurationSettings _instance = null;
        private static readonly object padlock = new object();
        private static IConfiguration configuration;

        #region Object Creation
        private ConfigurationSettings()
        {
            configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                    .Build();
        }

        public static ConfigurationSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationSettings();
                    }
                    return _instance;
                }
            }
        }

        #endregion

        public string APIURL
        {
            get => configuration.GetValue<string>("APIURL");
        }

        public string WebURL
        {
            get => configuration.GetValue<string>("WebURL");
        }

        public string UploadDirectoryPath
        {
            get => configuration.GetValue<string>("UploadDirectoryPath");
        }

        public string JWTKey
        {
            get => configuration.GetValue<string>("JWTKey");
        }

        public  double JWTExpireDays
        {
            get => Convert.ToDouble(configuration.GetValue<string>("JWTExpireDays"));
        }

        public double JWTRefreshTokenExpireDays
        {
            get => Convert.ToDouble(configuration.GetValue<string>("JWTRefreshTokenExpireDays"));
        }

        public  string JWTIssuer
        {
            get => configuration.GetValue<string>("JWTIssuer");
        }

        public string CookieName
        {
            get => configuration.GetValue<string>("CookieName");
        }

        public int EmailTokenExpirationDays
        {
            get => configuration.GetValue<int>("EmailTokenExpirationDays");
        }

        public string SyncFusionLicenseKey
        {
            get => configuration.GetValue<string>("SyncfusionLicenseKey");
        }

        public long MaxDocumentUploadSize
        {
            get => configuration.GetValue<long>("MaxDocumentUploadSize");
        }

        public long MaxProfileImageSize
        {
            get => configuration.GetValue<long>("MaxProfileImageSize");
        }

        public string SupportedDocuments
        {
            get => configuration.GetValue<string>("SupportedDocuments");
        }

        public string SupportedImageTypes
        {
            get => configuration.GetValue<string>("SupportedImageTypes");
        }

        public string AirportAPIURL
        {
            get => configuration.GetValue<string>("AirportAPIURL");
        }

        public MailSettingConfig MailSetting
        {
            get => MailSettingConfig.Instance;
        }

        public IEnumerable<int> BlazorGridPagesizeOptions = new int[] { 10, 20, 30, 50, 100 };

        public IEnumerable<int?> GridPagesizeOptions = new int?[] { 10, 20, 30, 50, 100, null };

        public int BlazorGridDefaultPagesize = 10;

        public bool IsDiplsayValidationInPopupEffect = false;

        public string  PagingSummaryFormat = "Displaying page {0} of {1} (total {2} records)";
    }
}
