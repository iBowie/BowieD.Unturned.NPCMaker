using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class HolidayManager
    {
        private static HashSet<DayMonth> days;
        static HolidayManager()
        {
            days = new HashSet<DayMonth>()
            {
                new DayMonth("Happy Birthday, BowieD!", 22, MAR),
                new DayMonth("Happy Birthday, DimesAO!", 30, NOV),
                new DayMonth("Happy New Year's Eve!", 31, DEC),
                new DayMonth("Happy New Year!", 1, JAN),
                new DayMonth("April Fools!", 1, APR),
                new DayMonth("Happy Birthday, Зефирка!", 7, MAY),
                new DayMonth("May the force be with you...", 4, APR),
                new DayMonth("Happy Birthday, Minecraft!", 17, MAY),
                new DayMonth("Life is Strange...", 11, OCT),
                new DayMonth("Happy Anniversary, Sonic The Hedgehog!", 23, JUN),
                new DayMonth("Happy Birthday, Terraria!", 16, MAY),
                new DayMonthYear("They CAN stop all of us...", 20, SEP, 2019),
                new DayMonthRange("Spooky!", 20, OCT, 1, NOV)
            };
        }
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
            foreach (var k in days)
                yield return k;
        }
        public static void Check()
        {
            int day = DateTime.Now.Day;
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            foreach (var k in Holidays())
            {
                if (k.Check())
                {
                    App.NotificationManager.Notify(k.Text);
                    k.OnCheck?.Invoke();
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
            public virtual bool Check()
            {
                return Day == DateTime.Now.Day && Month == DateTime.Now.Month;
            }
        }
        private class DayMonthYear : DayMonth
        {
            public DayMonthYear(string text, int day, int month, int year) : base(text, day, month)
            {
                this.Year = year;
            }
            public int Year { get; set; }
            public override bool Check()
            {
                return base.Check() && DateTime.Now.Year == Year;
            }
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
            public override bool Check()
            {
                var endDate = new DateTime(DateTime.Now.Year, EndMonth, EndDay);
                var startDate = new DateTime(DateTime.Now.Year, Month, Day);
                return (DateTime.Now > startDate && DateTime.Now < endDate);
            }
        }
    }
}
