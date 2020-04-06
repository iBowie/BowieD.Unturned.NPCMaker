using System;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class HolidayManager
    {
        public static void Check()
        {
            foreach (var holiday in App.Package.Holidays)
            {
                if (holiday.Range.IsInRange(DateTime.UtcNow))
                {
                    App.NotificationManager.Notify(holiday.Notification);
                }
            }
        }
    }
}
