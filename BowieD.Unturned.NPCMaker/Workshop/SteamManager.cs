using BowieD.Unturned.NPCMaker.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Workshop
{
    public interface ISteamManager
    {
        int CreateUGC(UGC mod, out ulong fileID);
    }
    public sealed class SteamManager : ISteamManager
    {
        public int CreateUGC(UGC mod, out ulong fileID)
        {
            List<string> psiArgs = new List<string>
            {
                $"name:{mod.Name}",
                $"desc:{mod.Description}",
                $"allowedIPs:{mod.AllowedIPs}",
                $"change:{mod.Change}",
                $"preview:{mod.Preview}",
                $"path:{mod.Path}",
                $"visibility:{mod.Visibility.ToString(CultureInfo.InvariantCulture)}"
            };

            ProcessStartInfo psi = new ProcessStartInfo(Path.Combine(AppConfig.Directory, "UnturnedWorkshopCLI.exe"), string.Join(" ", psiArgs.Select(d => $"\"{d}\"")))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            var p = Process.Start(psi);

            try
            {
                p.WaitForInputIdle();
            }
            catch { }
            p.WaitForExit();

            string pOut = p.StandardOutput.ReadToEnd();

            if (!ulong.TryParse(pOut, out fileID))
                fileID = 0;

            return p.ExitCode;
        }
    }

    public sealed class UGC
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Change { get; set; }
        public string Preview { get; set; }
        public string AllowedIPs { get; set; }
        public int Visibility { get; set; }
    }
}
