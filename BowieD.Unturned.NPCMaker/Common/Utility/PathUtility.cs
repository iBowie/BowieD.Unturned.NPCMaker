using System;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Common.Utility
{
    public static class PathUtility
    {
        public static bool IsUnturnedPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return File.Exists(Path.Combine(path, "Unturned.exe"));
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"Could not verify if ``{path}`` is Unturned folder.", ex: ex);
                return false;
            }
        }
        public static bool IsUnturnedWorkshopPath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return Directory.Exists(path) && path.Contains("304930") && path.Contains("workshop") && path.Contains("content");
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"Could not verify if ``{path}`` is Unturned workshop folder.", ex: ex);
                return false;
            }
        }
        public static string GetUnturnedWorkshopPathFromUnturnedPath(string unturnedPath)
        {
            DirectoryInfo dirInfo = Directory.GetParent(Directory.GetParent(unturnedPath).FullName);
            DirectoryInfo workshop = new DirectoryInfo(Path.Combine(dirInfo.FullName, "workshop", "content", "304930"));
            return workshop.FullName;
        }
    }
}
