using BowieD.Unturned.NPCMaker.Templating.Modify.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BowieD.Unturned.NPCMaker.Templating.Modify.Converters
{
    public sealed class ModifyValueConverter : JsonConverter
    {
        internal class TypeInfo
        {
            public TypeInfo(Type modifyType, Type innerType)
            {
                this.ModifyType = modifyType;
                this.InnerType = innerType;
            }

            public Type ModifyType { get; }
            public Type InnerType { get; }
        }

        public override bool CanWrite => false;

        internal static Dictionary<string, TypeInfo> Types { get; } = new Dictionary<string, TypeInfo>();

        public static void Register<T>()
        {
            string name;

            var attrib = typeof(T).GetCustomAttribute<ModifyValueAttribute>();

            if (attrib != null && !string.IsNullOrEmpty(attrib.ID))
                name = attrib.ID;
            else
                name = typeof(T).Name;

            Types[name] = new TypeInfo(typeof(T), attrib.InnerType ?? typeof(T));
        }
        public static void Register<T>(string id, Type innerType)
        {
            string name;

            if (!string.IsNullOrEmpty(id))
                name = id;
            else
                name = typeof(T).Name;

            Types[name] = new TypeInfo(typeof(T), innerType ?? typeof(T));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IModifyValue).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject j = JObject.Load(reader);

            if (j.TryGetValue("type", out var typetoken) && j.TryGetValue("value", out var valuetoken))
            {
                if (Types.TryGetValue(typetoken.Value<string>(), out var typeInfo))
                {
                    var js = new JsonSerializer();
                    js.Converters.Add(new StringEnumConverter());
                    var pjs = new JsonSerializer();
                    foreach (var c in serializer.Converters)
                        pjs.Converters.Add(c);

                    Type inType = typeInfo.InnerType;

                    object v = js.Deserialize(valuetoken.CreateReader(), inType);
                    IModifyValue[] p;
                    if (j.TryGetValue("parameter", out var parametertoken))
                    {
                        p = (IModifyValue[])pjs.Deserialize(parametertoken.CreateReader(), typeof(IModifyValue[]));
                    }
                    else
                        p = new IModifyValue[0];
                    ModifyEntry[] entries;
                    if (j.TryGetValue("modify", out var modifytoken))
                    {
                        entries = (ModifyEntry[])pjs.Deserialize(modifytoken.CreateReader(), typeof(ModifyEntry[]));
                    }
                    else
                        entries = new ModifyEntry[0];

                    IModifyValue imv = (IModifyValue)Activator.CreateInstance(typeInfo.ModifyType);

                    imv.Parameter = p;
                    imv.Value = v;
                    imv.Modify = entries;

                    return imv;
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
