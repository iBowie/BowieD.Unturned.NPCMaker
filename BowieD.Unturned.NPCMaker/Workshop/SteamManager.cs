using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Workshop
{
    public interface ISteamManager
    {
        Task<Tuple<int, ulong>> CreateUGC(UGC mod);
        Task<Tuple<int, ulong>> UpdateUGC(UGC mod);
        Task<IEnumerable<UGC>> QueryUGC();
    }
    public sealed class SteamManager : ISteamManager
    {
        public async Task<Tuple<int, ulong>> CreateUGC(UGC mod)
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

            var p = await RunProcessAsync(psi);

            string pOut = p.Item2;

            ulong fileID = 0;
            using (StringReader sr = new StringReader(pOut))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (ulong.TryParse(line, out fileID))
                        break;
                }
            }

            Tuple<int, ulong> tuple = new Tuple<int, ulong>(p.Item1, fileID);

            return tuple;
        }

        public async Task<IEnumerable<UGC>> QueryUGC()
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

            var p = await RunProcessAsync(psi);

            List<UGC> result = new List<UGC>();

            if (p.Item1 == 0)
            {
                string pOut = p.Item2;

                using (TextReader tr = new StringReader(pOut))
                {
                    while (true)
                    {
                        string line = tr.ReadLine();
                        if (string.IsNullOrEmpty(line))
                            break;

                        UGC res = new UGC();

                        if (!ulong.TryParse(line, out var fileId))
                            continue;

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
                        res.Preview = tr.ReadLine();

                        result.Add(res);
                    }
                }
            }

            return result;
        }

        public async Task<Tuple<int, ulong>> UpdateUGC(UGC mod)
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

            var p = await RunProcessAsync(psi);

            string pOut = p.Item2;

            ulong fileID = 0;
            using (StringReader sr = new StringReader(pOut))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (ulong.TryParse(line, out fileID))
                        break;
                }
            }

            Tuple<int, ulong> tuple = new Tuple<int, ulong>(p.Item1, fileID);

            return tuple;
        }

        static async Task<Tuple<int, string>> RunProcessAsync(ProcessStartInfo psi)
        {
            var process = new Process()
            {
                StartInfo = psi,
                EnableRaisingEvents = true
            };

            var tcs = new TaskCompletionSource<int>();

            process.Exited += (sender, e) =>
            {
                tcs.TrySetResult(process.ExitCode);
            };

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            int exitCode = await tcs.Task;
            
            process.Dispose();

            return new Tuple<int, string>(exitCode, output);
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
