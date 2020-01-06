using System;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Common.Utility
{
    public static class PathUtility
    {
        public static bool IsUnturnedPath(string path)
        {
            return File.Exists(Path.Combine(path, "Unturned.exe"));
        }
        public static bool IsUnturnedWorkshopPath(string path)
        {
            return Directory.Exists(path) && path.Contains("304930") && path.Contains("workshop") && path.Contains("content");
        }
        public static string GetUnturnedWorkshopPathFromUnturnedPath(string unturnedPath)
        {
            var dirInfo = Directory.GetParent(Directory.GetParent(unturnedPath).FullName);
            var workshop = new DirectoryInfo(Path.Combine(dirInfo.FullName, "workshop", "content", "304930"));
            return workshop.FullName;
        }
    }
}
