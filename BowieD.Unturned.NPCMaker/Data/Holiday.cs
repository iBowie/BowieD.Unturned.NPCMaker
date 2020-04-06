using Newtonsoft.Json;

namespace BowieD.Unturned.NPCMaker.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Holiday
    {
        [JsonConstructor]
        public Holiday(string notification, DateTimeRange range)
        {
            this.Notification = notification;
            this.Range = range;
        }
        [JsonProperty("notification")]
        public string Notification { get; }
        [JsonProperty("range")]
        public DateTimeRange Range { get; }
    }
}
