using BowieD.Unturned.NPCMaker.Templating.Conditions.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Templating.Conditions.Converters
{
    public sealed class TemplateConditionConverter : JsonConverter
    {
        public override bool CanWrite => false;

        static Dictionary<string, Type> Types { get; } = new Dictionary<string, Type>();

        public static void Register<T>() where T : ITemplateCondition
        {
            string name;

            var attrib = typeof(T).GetCustomAttribute<TemplateConditionAttribute>();

            if (attrib != null && !string.IsNullOrEmpty(attrib.ID))
                name = attrib.ID;
            else
                name = typeof(T).Name;

            Types[name] = typeof(T);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ITemplateCondition).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject j = JObject.Load(reader);

            if (j.TryGetValue("type", out var typetoken))
            {
                if (Types.TryGetValue(typetoken.Value<string>(), out var type))
                {
                    return new JsonSerializer().Deserialize(j.CreateReader(), type);
                }
            }

            return new TemplateCondition_AlwaysTrue();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
