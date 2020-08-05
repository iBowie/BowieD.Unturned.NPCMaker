using BowieD.Unturned.NPCMaker.Templating.Modify.Attributes;
using Newtonsoft.Json;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModifyValue("boolean", typeof(bool))]
    public sealed class ModifyValue_Boolean : IModifyValue
    {
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("parameter")]
        public IModifyValue[] Parameter { get; set; }
        [JsonProperty("modify")]
        public ModifyEntry[] Modify { get; set; }

        public object GetObject(Template template)
        {
            var o = Value;
            ModifyTool.ApplyModify(template, Modify, o);
            return o;
        }
    }
}
