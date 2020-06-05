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

            if (Annual)
            {
                int fromDeltaYears = DateTime.UtcNow.Year - From.Year;
                int toDeltaYears = DateTime.UtcNow.Year - To.Year;

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
