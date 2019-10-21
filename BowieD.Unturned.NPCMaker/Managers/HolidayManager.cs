using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class HolidayManager
    {
        private const int
            JAN = 1,
            FEB = 2,
            MAR = 3,
            APR = 4,
            MAY = 5,
            JUN = 6,
            JUL = 7,
            AUG = 8,
            SEP = 9,
            OCT = 10,
            NOV = 11,
            DEC = 12;
        private static IEnumerable<DayMonth> Holidays()
        {
            yield return new DayMonth("Happy Birthday, BowieD!", 22, MAR);
            yield return new DayMonth("Happy Birthday, DimesAO!", 30, NOV);
            yield return new DayMonth("Happy New Year's Eve!", 31, DEC);
            yield return new DayMonth("Happy New Year!", 1, JAN);
            yield return new DayMonth("April Fools!", 1, APR);
            yield return new DayMonth("Happy Birthday, Зефирка!", 7, MAY);
            yield return new DayMonth("May the force be with you...", 4, APR);
            yield return new DayMonth("Happy Birthday, Minecraft!", 17, MAY);
            yield return new DayMonth("Life is Strange...", 11, OCT);
            yield return new DayMonth("Happy Anniversary, Sonic The Hedgehog!", 23, JUN);
            yield return new DayMonth("Happy Birthday, Terraria!", 16, MAY);
            yield return new DayMonthYear("They CAN stop all of us...", 20, SEP, 2019);
            yield return new DayMonthRange("Spooky!", 20, OCT, 1, NOV);
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
                        App.NotificationManager.Notify(dmy.Text);
                        dmy.OnCheck?.Invoke();
                    }
                }
                else if (k is DayMonthRange dmr)
                {
                    var endDate = new DateTime(year, dmr.EndMonth, dmr.EndDay);
                    var startDate = new DateTime(year, dmr.Month, dmr.Day);
                    if (DateTime.Now > startDate && DateTime.Now < endDate)
                    {
                        App.NotificationManager.Notify(dmr.Text);
                        dmr.OnCheck?.Invoke();
                    }
                }
                else
                {
                    if (k.Day == day && k.Month == month)
                    {
                        App.NotificationManager.Notify(k.Text);
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
                this.Text = text;
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
        private class DayMonthRange : DayMonth
        {
            public DayMonthRange(string text, int day, int month, int endDay, int endMonth) : base(text, day, month)
            {
                EndDay = endDay;
                EndMonth = endMonth;
            }
            public int EndDay { get; set; }
            public int EndMonth { get; set; }
        }
    }
}
