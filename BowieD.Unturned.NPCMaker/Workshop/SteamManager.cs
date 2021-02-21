using BowieD.Unturned.NPCMaker.Configuration;
using System;
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
        int UpdateUGC(UGC mod, out ulong fileID);
        IEnumerable<UGC> QueryUGC();
    }
    public sealed class SteamManager : ISteamManager
    {
        public int CreateUGC(UGC mod, out ulong fileID)
        {
            List<string> psiArgs = new List<string>
            {
                $"mode:create",
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

        public IEnumerable<UGC> QueryUGC()
        {
            List<string> psiArgs = new List<string>
            {
                "mode:query"
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

            if (p.ExitCode == 0)
            {
                string pOut = p.StandardOutput.ReadToEnd();

                App.Logger.Log($"pOut\n{pOut}");

                using (TextReader tr = new StringReader(pOut))
                {
                    while (true)
                    {
                        string line = tr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;

                        UGC res = new UGC();

                        if (!ulong.TryParse(line, out var fileId))
                            throw new System.Exception("Invalid input");

                        res.FileID = fileId;

                        switch (tr.ReadLine())
                        {
                            case "public":
                                res.Visibility = 0;
                                break;
                            case "friends":
                                res.Visibility = 1;
                                break;
                            case "unlisted":
                                res.Visibility = 2;
                                break;
                            case "private":
                                res.Visibility = 3;
                                break;
                        }

                        res.Name = tr.ReadLine().Replace("<br>", Environment.NewLine);
                        
                        yield return res;
                    }
                }
            }
        }

        public int UpdateUGC(UGC mod, out ulong fileID)
        {
            List<string> psiArgs = new List<string>
            {
                $"mode:update",
                $"fileid:{mod.FileID}",
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
        public ulong FileID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Change { get; set; }
        public string Preview { get; set; }
        public string AllowedIPs { get; set; }
        public int Visibility { get; set; }
    }
}
