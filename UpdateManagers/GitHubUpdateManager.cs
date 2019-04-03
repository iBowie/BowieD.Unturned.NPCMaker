using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public class GitHubUpdateManager : IUpdateManager
    {
        public UpdateAvailability UpdateAvailability { get; set; } = UpdateAvailability.NOT_CHECKED;
        private static async void DownloadUpdater()
        {
            using (WebClient wc = new WebClient())
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", FileMode.Create))
            {
                byte[] dat = await wc.DownloadDataTaskAsync("https://raw.githubusercontent.com/iBowie/publicfiles/master/BowieD.Unturned.NPCMaker.Updater.exe");
                for (int k = 0; k < dat.Length; k++)
                {
                    fs.WriteByte(dat[k]);
                }
                Logger.Log("Updater downloaded!");
            }
        }
        public void StartUpdate()
        {
            DownloadUpdater();
            string fileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!fileName.EndsWith(".exe"))
                fileName += ".exe";
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", fileName);
            Environment.Exit(0);
        }
        public async Task<UpdateAvailability> CheckForUpdates()
        {
            Logger.Log("Update check started");
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string vers = await wc.DownloadStringTaskAsync("https://raw.githubusercontent.com/iBowie/publicfiles/master/npcmakerversion.txt");
                    Version newVersion = new Version(vers);
                    bool res = newVersion > MainWindow.Version;
                    if (res)
                        Logger.Log("Update available.");
                    else
                        Logger.Log("No updates available.");
                    var result = res ? UpdateAvailability.AVAILABLE : UpdateAvailability.NOT_AVAILABLE;
                    if (UpdateAvailability == UpdateAvailability.NOT_CHECKED)
                        UpdateAvailability = result;
                    return result;
                }
            }
            catch { Logger.Log("Update check failed!"); UpdateAvailability = UpdateAvailability.ERROR; return UpdateAvailability.ERROR; }
        }
    }
}
