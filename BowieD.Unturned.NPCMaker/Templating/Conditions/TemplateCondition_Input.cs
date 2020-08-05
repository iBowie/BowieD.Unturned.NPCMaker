using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Templating.Conditions.Attributes;
using BowieD.Unturned.NPCMaker.Templating.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace BowieD.Unturned.NPCMaker.Templating.Conditions
{
    [TemplateCondition("input")]
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class TemplateCondition_Input : ITemplateCondition
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("logic")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Logic_Type Logic { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }

        public bool IsMet(Template template)
        {
            var type = TypeResolver.Resolve("input", Field, template);
            var value = template.UserInputs[Field];

            bool isComparable = typeof(IComparable).IsAssignableFrom(type);

            int? compareResult;

            if (isComparable && value is IComparable comparableGT)
            {
                try
                {
                    var v = Convert.ChangeType(Value, value.GetType());
                    compareResult = comparableGT.CompareTo(v);
                }
                catch
                {
                    compareResult = null;
                }
            }
            else
            {
                compareResult = null;
            }

            switch (Logic)
            {
                case Logic_Type.Equal:
                    if (compareResult.HasValue)
                        return compareResult == 0;
                    return Value.Equals(value);
                case Logic_Type.Not_Equal:
                    if (compareResult.HasValue)
                        return compareResult != 0;
                    return !Value.Equals(value);
                case Logic_Type.Greater_Than when compareResult.HasValue:
                    return compareResult > 0;
                case Logic_Type.Greater_Than_Or_Equal_To when compareResult.HasValue:
                    return compareResult >= 0;
                case Logic_Type.Less_Than when compareResult.HasValue:
                    return compareResult < 0;
                case Logic_Type.Less_Than_Or_Equal_To when compareResult.HasValue:
                    return compareResult <= 0;
                default:
                    throw new Exception();
            }
        }
    }
}
