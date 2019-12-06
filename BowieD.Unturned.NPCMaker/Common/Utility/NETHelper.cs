using Microsoft.Win32;
using System;

namespace BowieD.Unturned.NPCMaker.Common.Utility
{
    public static class NETHelper
    {
        static readonly NETVersion version;
        public static string GetVersionString()
        {
            return GetVersion().ToString().Replace("_", ".");
        }
        public static NETVersion GetVersion() => version;
        static NETHelper()
        {
            int dotNetVersion;
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            using (RegistryKey ndpKey = baseKey.OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    dotNetVersion = (int)ndpKey.GetValue("Release");
                }
                else
                {
                    dotNetVersion = 0;
                }
            }
            foreach (var ver in Enum.GetValues(typeof(NETVersion)))
            {
                var nv = (NETVersion)ver;
                if (dotNetVersion >= (int)nv)
                    version = nv;
            }
        }
    }
    public enum NETVersion
    {
        none = 0,
        v4_5 = 378389,
        v4_5_1 = 378675,
        v4_5_2 = 379893,
        v4_6 = 393295,
        v4_6_1 = 394254,
        v4_6_2 = 394802,
        v4_7 = 460798,
        v4_7_1 = 461308,
        /// <summary>
        /// REQUIRED BY THIS VERSION
        /// </summary>
        v4_7_2 = 461808,
        v4_8 = 528040
    }
}
