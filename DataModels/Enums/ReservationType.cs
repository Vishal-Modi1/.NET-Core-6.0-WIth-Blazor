using System;
using System.ComponentModel;
using System.Reflection;

namespace DataModels.Enums
{
    public enum ReservationType
    {
        [Description("Past")]
        Past = 1,
        [Description("Future")]
        Future = 2,
        [Description("Cancelled")]
        Cancelled = 3,
        [Description("Completed/Checked In")]
        Completed_CheckedIn = 4
    }
}
