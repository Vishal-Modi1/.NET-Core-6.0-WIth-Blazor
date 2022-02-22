using Radzen;

namespace FSM.Blazor.Extensions
{
    public static class NotificationContainsBuilder
    {
        public static NotificationMessage Build(this NotificationMessage builder, NotificationSeverity severity, string summary, string message)
        {
            return new NotificationMessage {Severity = severity, Summary = summary, Detail = message, Duration = 4000 };
        }
    }
}
