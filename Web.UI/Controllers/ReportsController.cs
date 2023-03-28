using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.WebReportDesigner.Services;
using Telerik.WebReportDesigner.Services.Controllers;

namespace Web.UI.Controllers
{
    [Serializable]
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
              : base(reportServiceConfiguration)
        {
            //var connectionString = "Data Source=(local);Initial Catalog=AdventureWorks;Integrated Security=SSPI";
            //var connectionStringHandler = new ReportConnectionStringManager(connectionString);
            //var sourceReportSource = new UriReportSource { Uri = "Employee Sales Summary.trdx" };
            ////var sourceReportSource = new InstanceReportSource { ReportDocument = new EmployeeSalesSummary() };
            //var reportSource = connectionStringHandler.UpdateReportSource(sourceReportSource);
            //this.reportViewer1.ReportSource = reportSource;
            //this.reportViewer1.RefreshReport();

        }

        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            throw new System.NotImplementedException("This method should be implemented in order to send mail messages");

            // using (var smtpClient = new SmtpClient("smtp01.mycompany.com", 25))
            // {
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtpClient.EnableSsl = false;

            // smtpClient.Send(mailMessage);
            // }
            // return HttpStatusCode.OK;
        }
    }
}