using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.Templating.Modify.Attributes;
using BowieD.Unturned.NPCMaker.Templating.Reflection;
using Newtonsoft.Json;
using System;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModifyValue("condition", typeof(string))]
    public sealed class ModifyValue_Condition : IModifyValue
    {
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("parameter")]
        public IModifyValue[] Parameter { get; set; }
        [JsonProperty("modify")]
        public ModifyEntry[] Modify { get; set; }

        public object GetObject(Template template)
        {
            var cType = TypeResolver.Resolve("condition", Value, template);
            Condition c = (Condition)Activator.CreateInstance(cType);
            ModifyTool.ApplyModify(template, Modify, c);
            return c;
        }
    }
}
