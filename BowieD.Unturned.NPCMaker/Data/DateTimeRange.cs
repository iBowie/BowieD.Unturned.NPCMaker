using Newtonsoft.Json;
using System;

namespace BowieD.Unturned.NPCMaker.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public struct DateTimeRange
    {
        public DateTimeRange(DateTime from, DateTime to, bool annual = false)
        {
            From = from;
            To = to;
            Annual = annual;
        }
        [JsonProperty("from")]
        public DateTime From { get; }
        [JsonProperty("to")]
        public DateTime To { get; }
        [JsonProperty("annual")]
        public bool Annual { get; }

        public bool IsInRange(DateTime dateTime)
        {
            DateTime checkFrom, checkTo;

            int fromDeltaYears = DateTime.UtcNow.Year - From.Year;
            int toDeltaYears = DateTime.UtcNow.Year - To.Year;

            if (Annual && fromDeltaYears >= 0 && toDeltaYears >= 0)
            {
                checkFrom = From.AddYears(fromDeltaYears);
                checkTo = To.AddYears(toDeltaYears);
            }
            else
            {
                checkFrom = From;
                checkTo = To;
            }
            return dateTime > checkFrom && dateTime < checkTo;
        }
    }
}
