using DataModels.VM.Common;
using Telerik.Blazor.Components;
using Web.UI.Extensions;
using static Telerik.Blazor.ThemeConstants;

namespace Web.UI.Shared
{
    public class UINotification
    {
        public TelerikNotification Instance { get; set; }
        NotificationModel message;

        public void DisplayNotification(TelerikNotification instance, CurrentResponse response)
        {
            if (response == null)
            {
                DisplayErrorNotification(instance);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
               DisplaySuccessNotification(instance,response.Message);
            }
            else
            {
                DisplayCustomErrorNotification(instance, response.Message);
            }
        }

        public void DisplayInfoNotification(TelerikNotification instance, string messageText)
        {
            instance.HideAll();

            message = new NotificationModel().Build(Notification.ThemeColor.Info, messageText);
            instance.Show(message);
        }

        public void DisplaySuccessNotification(TelerikNotification instance, string messageText)
        {
            instance.HideAll();

            message = new NotificationModel().Build(Notification.ThemeColor.Success, messageText);
            instance.Show(message);
        }

        public void DisplayErrorNotification(TelerikNotification instance)
        {
            instance.HideAll();

            message = new NotificationModel().Build(Notification.ThemeColor.Error, "Something went Wrong!, Please try again later.");
            instance.Show(message);
        }

        public void DisplayCustomErrorNotification(TelerikNotification instance, string messageText)
        {
            instance.HideAll();

            message = new NotificationModel().Build(Notification.ThemeColor.Error, messageText);
            instance.Show(message);
        }
    }
}
