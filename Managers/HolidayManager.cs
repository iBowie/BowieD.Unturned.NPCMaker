using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class HolidayManager
    {
        private static IEnumerable<DayMonth> Holidays()
        {
            yield return new DayMonth("Happy Birthday, BowieD!", 22, 3);
            yield return new DayMonth("Happy Birthday, DimesAO!", 30, 11);
            yield return new DayMonth("Happy New Year's Eve!", 31, 12);
            yield return new DayMonth("Happy New Year!", 1, 1);
            yield return new DayMonth("April Fools!", 1, 4);
            yield return new DayMonth("Happy Birthday, Зефирка!", 7, 5);
            yield return new DayMonth("May the force be with you...", 4, 4);
            yield return new DayMonth("Happy Birthday, Minecraft!", 17, 5);
            yield return new DayMonth("Life is Strange...", 11, 10);
            yield return new DayMonth("Happy Anniversary, Sonic The Hedgehog!", 23, 6);
        }
        public static void Check()
        {
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            foreach (var k in Holidays())
            {
                if (k is DayMonthYear dmy)
                {
                    if (dmy.Day == day && dmy.Month == month && dmy.Year == year)
                    {
                        MainWindow.NotificationManager.Notify(dmy.Text);
                        dmy.OnCheck?.Invoke();
                    }
                }
                else
                {
                    if (k.Day == day && k.Month == month)
                    {
                        MainWindow.NotificationManager.Notify(k.Text);
                        k.OnCheck?.Invoke();
                    }
                }
            }
        }
        private class DayMonth
        {
            public string Text { get; set; }
            public DayMonth(string text, int day, int month)
            {
                this.Day = day;
                this.Month = month;
            }
            public int Day { get; set; }
            public int Month { get; set; }
            public Action OnCheck { get; }
        }
        private class DayMonthYear : DayMonth
        {
            public DayMonthYear(string text, int day, int month, int year) : base(text, day, month)
            {
                this.Year = year;
            }
            public int Year { get; set; }
        }
    }
}
