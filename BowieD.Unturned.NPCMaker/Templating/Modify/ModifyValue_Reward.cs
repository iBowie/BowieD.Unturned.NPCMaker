using BowieD.Unturned.NPCMaker.NPC.Rewards;
using BowieD.Unturned.NPCMaker.Templating.Modify.Attributes;
using BowieD.Unturned.NPCMaker.Templating.Reflection;
using Newtonsoft.Json;
using System;

namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModifyValue("reward", typeof(string))]
    public sealed class ModifyValue_Reward : IModifyValue
    {
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("parameter")]
        public IModifyValue[] Parameter { get; set; }
        [JsonProperty("modify")]
        public ModifyEntry[] Modify { get; set; }

        public object GetObject(Template template)
        {
            var rType = TypeResolver.Resolve("reward", Value, template);
            Reward r = (Reward)Activator.CreateInstance(rType);
            ModifyTool.ApplyModify(template, Modify, r);
            return r;
        }
    }
}
