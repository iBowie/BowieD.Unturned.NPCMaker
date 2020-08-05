using BowieD.Unturned.NPCMaker.Templating.Modify;
using BowieD.Unturned.NPCMaker.Templating.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Templating
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Template
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("input")]
        public Dictionary<string, InputField> Inputs { get; set; }
        [JsonProperty("modify")]
        public ModifyEntry[] Modify { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        public Dictionary<string, object> UserInputs { get; } = new Dictionary<string, object>();
        public object FinalObject { get; set; }

        /// <summary>
        /// Setups template
        /// </summary>
        public void Init()
        {
            foreach (var i in Inputs)
                UserInputs.Add(i.Key, i.Value.Default);

            FinalObject = Activator.CreateInstance(TypeResolver.Resolve(Type, null, this));
        }
        /// <summary>
        /// Applies all modify operations
        /// </summary>
        public void Apply()
        {
            ModifyTool.ApplyModify(this, Modify, FinalObject);
        }
    }
}
