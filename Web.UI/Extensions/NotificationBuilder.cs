using Telerik.Blazor.Components;
using Web.UI.Models.Enums;

namespace Web.UI.Extensions
{
    public static class NotificationBuilder
    {
        public static NotificationModel Build(this NotificationModel builder, TelerikNotificationTypes type , string message, int duration = 5000)
        {
            return new NotificationModel { ThemeColor = type.ToString(), Text = message,  CloseAfter = duration };
        }
    }
}
