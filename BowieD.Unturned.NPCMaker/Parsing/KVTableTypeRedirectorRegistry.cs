using BowieD.Unturned.NPCMaker.GameIntegration;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public static class KVTableTypeRedirectorRegistry
    {
        private static Dictionary<string, string> redirects;

        public static string chase(string assemblyQualifiedName)
        {
            if (redirects.TryGetValue(assemblyQualifiedName, out string value))
            {
                return chase(value);
            }
            return assemblyQualifiedName;
        }

        public static void add(string oldAssemblyQualifiedName, string newAssemblyQualifiedName)
        {
            redirects.Add(oldAssemblyQualifiedName, newAssemblyQualifiedName);
        }

        public static void remove(string oldAssemblyQualifiedName)
        {
            redirects.Remove(oldAssemblyQualifiedName);
        }

        static KVTableTypeRedirectorRegistry()
        {
            redirects = new Dictionary<string, string>();

            add("SDG.Unturned.ItemCurrencyAsset, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(GameCurrencyAsset).AssemblyQualifiedName);
            add("SDG.Unturned.WeatherAsset, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(GameWeatherAsset).AssemblyQualifiedName);
        }
    }
}
