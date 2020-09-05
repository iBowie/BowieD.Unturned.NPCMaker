using BowieD.Unturned.NPCMaker.Templating.Conditions;
using BowieD.Unturned.NPCMaker.Templating.Modify.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class ModifyEntry
    {
        [JsonProperty("conditions")]
        public ITemplateCondition[] Conditions { get; set; }
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("operation")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EModifyEntryOperation Operation { get; set; }
        [JsonProperty("value")]
        [JsonConverter(typeof(ModifyValueConverter))]
        public IModifyValue Value { get; set; }
        [JsonProperty("expression")]
        public string Expression { get; set; }
    }
}
