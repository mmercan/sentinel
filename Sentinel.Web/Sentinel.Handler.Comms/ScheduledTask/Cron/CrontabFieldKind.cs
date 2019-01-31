using System;

namespace Sentinel.Handler.Comms.ScheduledTask.Cron
{
    [Serializable]
    public enum CrontabFieldKind
    {
        Minute,
        Hour,
        Day,
        Month,
        DayOfWeek
    }
}