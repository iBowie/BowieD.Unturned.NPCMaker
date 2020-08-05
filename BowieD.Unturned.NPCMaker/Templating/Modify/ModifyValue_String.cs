using BowieD.Unturned.NPCMaker.Templating.Modify.Attributes;
using Newtonsoft.Json;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModifyValue("string", typeof(string))]
    public sealed class ModifyValue_String : IModifyValue
    {
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("parameter")]
        public IModifyValue[] Parameter { get; set; }
        [JsonProperty("modify")]
        public ModifyEntry[] Modify { get; set; }

        public object GetObject(Template template)
        {
            var o = Value.ToString();
            o = string.Format(o, args: Parameter.Select(d => d.GetObject(template)).ToArray());
            ModifyTool.ApplyModify(template, Modify, o);
            return o;
        }
    }
}
