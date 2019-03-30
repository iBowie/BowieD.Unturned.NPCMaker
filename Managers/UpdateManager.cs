using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public class UpdateManager : IManager
    {
        public static UpdateManager Instance { get; private set; }

        public static void Create()
        {
            Instance = new UpdateManager();
        }

        public void Start()
        {
            updatesAvailable = IsUpdateAvailable().GetAwaiter().GetResult();
            End();
        }

        public void End()
        {
            Instance = null;
        }

        private static async Task<bool?> IsUpdateAvailable()
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
                    return res;
                }
            }
            catch { Logger.Log("Update check failed!"); return null; }
        }
        public static bool? updatesAvailable { get; private set; } = false;
        private static void DownloadUpdater()
        {
            using (WebClient wc = new WebClient())
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", FileMode.Create))
            {
                byte[] dat = wc.DownloadData("https://raw.githubusercontent.com/iBowie/publicfiles/master/BowieD.Unturned.NPCMaker.Updater.exe");
                for (int k = 0; k < dat.Length; k++)
                {
                    fs.WriteByte(dat[k]);
                }
                Logger.Log("Updater downloaded!");
            }
        }
        public static void RunUpdater()
        {
            DownloadUpdater();
            string fileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!fileName.EndsWith(".exe"))
                fileName += ".exe";
            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "updater.exe", fileName);
        }
    }
}
