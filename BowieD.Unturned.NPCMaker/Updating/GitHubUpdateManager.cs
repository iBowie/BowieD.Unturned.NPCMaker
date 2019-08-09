using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Updating
{
    public class GitHubUpdateManager : IUpdateManager
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public const string ReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker/releases/latest";
        public const string UpdaterReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker.Updater/releases/latest";
        public UpdateAvailability UpdateAvailability { get; set; } = UpdateAvailability.NOT_CHECKED;
        private static void DownloadUpdater()
        {
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(UpdaterReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                var respStream = httpRequest.GetResponse().GetResponseStream();

                if (respStream == null)
                {
                    throw new Exception("request returned null response");
                }

                using (var reader = new StreamReader(respStream))
                {
                    var jsonData = reader.ReadToEnd();
                    var jsonObj = JObject.Parse(jsonData);

                    var assets = jsonObj.GetValue("assets").Children<JObject>();

                    using (WebClient wc = new WebClient())
                    using (FileStream fs = new FileStream(AppConfig.Directory + "updater.exe", FileMode.Create))
                    {
                        byte[] data = wc.DownloadData(assets.First().GetValue("browser_download_url").ToString());
                        fs.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Could not download updater!", Log_Level.Error);
                Logger.Log(ex, Log_Level.Error);
            }
        }
        public void StartUpdate()
        {
            DownloadUpdater();
            string fileName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (!fileName.EndsWith(".exe"))
                fileName += ".exe";
            System.Diagnostics.Process.Start(AppConfig.Directory + "updater.exe", fileName);
            MainWindow.PerformExit();
        }
        public async Task<UpdateAvailability> CheckForUpdates()
        {
            try
            {
                var manifest = await GetManifest();
                Version latestVers = Version.Parse(manifest.Value.tag_name);
                Title = manifest.Value.name;
                Content = manifest.Value.body;
                if (latestVers > MainWindow.Version)
                    return UpdateAvailability.AVAILABLE;
                else
                    return UpdateAvailability.NOT_AVAILABLE;
            }
            catch
            {
                return UpdateAvailability.ERROR;
            }
        }

        private async Task<UpdateManifest?> GetManifest()
        {
            File.Delete(AppConfig.Directory + "update.manifest");
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(ReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                var webResponse = await httpRequest.GetResponseAsync();
                var respStream = webResponse.GetResponseStream();

                if (respStream == null)
                {
                    return null;
                }
                UpdateManifest? manifest = null;
                using (var reader = new StreamReader(respStream))
                {
                    var jsonData = reader.ReadToEnd();
                    manifest = JsonConvert.DeserializeObject<UpdateManifest>(jsonData);
                }
                File.WriteAllText(AppConfig.Directory + "update.manifest", JsonConvert.SerializeObject(manifest));
                return manifest;
            }
            catch (Exception ex)
            {
                Logger.Log("Could not check update!", Log_Level.Error);
                Logger.Log(ex, Log_Level.Error);
                return null;
            }
        }
        public struct UpdateManifest
        {
            public string name;
            public string tag_name;
            public string body;
            public asset[] assets;
            public class asset
            {
                public string browser_download_url;
            }
        }
    }
}
