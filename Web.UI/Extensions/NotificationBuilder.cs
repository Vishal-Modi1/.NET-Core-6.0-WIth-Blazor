using Telerik.Blazor.Components;

namespace Web.UI.Extensions
{
    public static class NotificationBuilder
    {
        public static NotificationModel Build(this NotificationModel builder, string type , string message, int duration = 1699999999)
        {
            return new NotificationModel { ThemeColor = type, Text = message,  CloseAfter = duration };
        }
    }
}
