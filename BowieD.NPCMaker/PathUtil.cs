using System;
using System.IO;

namespace BowieD.NPCMaker
{
    public static class PathUtil
    {
        public static string GetWorkDir()
        {
#if PORTABLE
            return AppDomain.CurrentDomain.BaseDirectory;
#elif true
            char psc = Path.DirectorySeparatorChar;
            return $"C{Path.VolumeSeparatorChar}Users{psc}{Environment.UserName}{psc}AppData{psc}Local{psc}BowieD{psc}NPCMaker2{psc}";
#endif
        }
    }
}
