using Newtonsoft.Json;

namespace BowieD.Unturned.NPCMaker.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class FeedbackLink
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("loc")]
        public bool Localize { get; set; }
        [JsonProperty("url")]
        public string URL { get; set; }
    }
}
