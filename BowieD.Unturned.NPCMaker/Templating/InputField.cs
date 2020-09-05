using Newtonsoft.Json;

namespace BowieD.Unturned.NPCMaker.Templating
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class InputField
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("default")]
        public object Default { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("tooltip")]
        public string ToolTip { get; set; }
    }
}
