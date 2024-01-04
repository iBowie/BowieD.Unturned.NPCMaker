using System;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class HolidayManager
    {
        public static void Check()
        {
            if (App.Package is null) return;
            if (App.Package.Holidays is null) return;

            foreach (Data.AppPackage.Holiday holiday in App.Package.Holidays)
            {
                if (holiday is null) continue;
                if (string.IsNullOrWhiteSpace(holiday.Notification)) continue;

                if (holiday.Range.IsInRange(DateTime.UtcNow))
                {
                    App.NotificationManager.Notify(holiday.Notification);
                }
            }
        }
    }
}
