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
        private static bool DownloadUpdater()
        {
            App.Logger.LogInfo("[UPDATE] - Downloading updater");
            try
            {
                App.Logger.LogInfo("[UPDATE] - Creating HTTP request to GitHub...");
                var httpRequest = (HttpWebRequest)WebRequest.Create(UpdaterReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                App.Logger.LogInfo("[UPDATE] - Waiting for response...");
                var respStream = httpRequest.GetResponse().GetResponseStream();
                App.Logger.LogInfo("[UPDATE] - Reading response...");

                if (respStream == null)
                {
                    App.Logger.LogInfo("[UPDATE] - Response is empty");
                    throw new Exception("request returned null response");
                }

                using (var reader = new StreamReader(respStream))
                {
                    var jsonData = reader.ReadToEnd();
                    var jsonObj = JObject.Parse(jsonData);

                    var assets = jsonObj.GetValue("assets").Children<JObject>();
                    App.Logger.LogInfo("[UPDATE] - Downloading updater");

                    using (WebClient wc = new WebClient())
                    using (FileStream fs = new FileStream(AppConfig.Directory + "updater.exe", FileMode.Create))
                    {
                        byte[] data = wc.DownloadData(assets.First().GetValue("browser_download_url").ToString());
                        fs.Write(data, 0, data.Length);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                App.Logger.LogException("[UPDATE] - Could not download updater!", ex);
                return false;
            }
        }
        public void StartUpdate()
        {
            if (DownloadUpdater())
            {
                string fileName = Environment.GetCommandLineArgs()[0];
                App.Logger.LogInfo("[UPDATE] - Launching updater");
                System.Diagnostics.Process.Start(AppConfig.Directory + "updater.exe", fileName);
                MainWindow.PerformExit();
            }
            else
            {
                App.Logger.LogWarning("[UPDATE] - Could not start update. Missing updater.");
            }
        }
        public async Task<UpdateAvailability> CheckForUpdates()
        {
            try
            {
                App.Logger.LogInfo("[UPDATE] - Getting update manifest...");
                var manifest = await GetManifest();
                App.Logger.LogInfo("[UPDATE] - Got update manifest");
                Version latestVers = Version.Parse(manifest.Value.tag_name);
                Title = manifest.Value.name;
                Content = manifest.Value.body;
                if (latestVers > App.Version)
                {
                    App.Logger.LogInfo("[UPDATE] - Newer version available");
                    return UpdateAvailability.AVAILABLE;
                }
                else
                {
                    App.Logger.LogInfo("[UPDATE] - User has latest or newer version already");
                    return UpdateAvailability.NOT_AVAILABLE;
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException("[UPDATE] - Could not check for updates", ex);
                return UpdateAvailability.ERROR;
            }
        }

        private async Task<UpdateManifest?> GetManifest()
        {
            if (File.Exists(AppConfig.Directory + "update.manifest"))
            {
                App.Logger.LogInfo("[UPDATE] - Deleting old update manifest...");
                File.Delete(AppConfig.Directory + "update.manifest");
                App.Logger.LogInfo("[UPDATE] - Deleted");
            }
            try
            {
                App.Logger.LogInfo("[UPDATE] - Creating HTTP request to GitHub...");
                var httpRequest = (HttpWebRequest)WebRequest.Create(ReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                App.Logger.LogInfo("[UPDATE] - Waiting for response...");
                var webResponse = await httpRequest.GetResponseAsync();
                App.Logger.LogInfo("[UPDATE] - Reading response...");
                var respStream = webResponse.GetResponseStream();

                if (respStream == null)
                {
                    App.Logger.LogWarning("[UPDATE] - Response is empty");
                    return null;
                }
                App.Logger.LogInfo("[UPDATE] - Response is not empty");
                UpdateManifest? manifest = null;
                using (var reader = new StreamReader(respStream))
                {
                    App.Logger.LogInfo("[UPDATE] - Converting response to manifest");
                    var jsonData = reader.ReadToEnd();
                    manifest = JsonConvert.DeserializeObject<UpdateManifest>(jsonData);
                    App.Logger.LogInfo("[UPDATE] - Converted");
                }
                App.Logger.LogInfo("[UPDATE] - Saving update manifest...");
                File.WriteAllText(AppConfig.Directory + "update.manifest", JsonConvert.SerializeObject(manifest));
                App.Logger.LogInfo("[UPDATE] - Saved");
                return manifest;
            }
            catch (Exception ex)
            {
                App.Logger.LogException("[UPDATE] - Could not check for updates!", ex);
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
