using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
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
        public const string ReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker/releases/latest";
        public const string UpdaterReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker.Updater/releases/latest";
        private static bool DownloadUpdater()
        {
            App.Logger.Log("[UPDATE] - Downloading updater");
            try
            {
                App.Logger.Log("[UPDATE] - Creating HTTP request to GitHub...");
                var httpRequest = (HttpWebRequest)WebRequest.Create(UpdaterReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                App.Logger.Log("[UPDATE] - Waiting for response...");
                var respStream = httpRequest.GetResponse().GetResponseStream();
                App.Logger.Log("[UPDATE] - Reading response...");

                if (respStream == null)
                {
                    App.Logger.Log("[UPDATE] - Response is empty");
                    throw new Exception("request returned null response");
                }

                using (var reader = new StreamReader(respStream))
                {
                    var jsonData = reader.ReadToEnd();
                    var jsonObj = JObject.Parse(jsonData);

                    var assets = jsonObj.GetValue("assets").Children<JObject>();
                    App.Logger.Log("[UPDATE] - Downloading updater");

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
                App.Logger.LogException("[UPDATE] - Could not download updater!", ex: ex);
                return false;
            }
        }
        public void StartUpdate()
        {
            if (DownloadUpdater())
            {
                string fileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                App.Logger.Log("[UPDATE] - Launching updater");
                System.Diagnostics.Process.Start(AppConfig.Directory + "updater.exe", $"\"{fileName}\"");
                Environment.Exit(0);
            }
            else
            {
                App.Logger.Log("[UPDATE] - Could not start update. Missing updater.");
            }
        }
        public async Task<UpdateAvailability> CheckForUpdates()
        {
            try
            {
                await App.Logger.Log("[UPDATE] - Getting update manifest...");
                var manifest = await GetManifest();
                await App.Logger.Log("[UPDATE] - Got update manifest");
                Version latestVers = Version.Parse(manifest.Value.tag_name);
                Whats_New.UpdateTitle = manifest.Value.name;
                Whats_New.UpdateContent = manifest.Value.body;
                if (latestVers > App.Version)
                {
                    await App.Logger.Log("[UPDATE] - Newer version available");
                    return UpdateAvailability.AVAILABLE;
                }
                else
                {
                    await App.Logger.Log("[UPDATE] - User has latest or newer version already");
                    return UpdateAvailability.NOT_AVAILABLE;
                }
            }
            catch (Exception ex)
            {
                await App.Logger.LogException("[UPDATE] - Could not check for updates", ex: ex);
                return UpdateAvailability.ERROR;
            }
        }

        private async Task<UpdateManifest?> GetManifest()
        {
            if (File.Exists(AppConfig.Directory + "update.manifest"))
            {
                await App.Logger.Log("[UPDATE] - Deleting old update manifest...");
                File.Delete(AppConfig.Directory + "update.manifest");
                await App.Logger.Log("[UPDATE] - Deleted");
            }
            try
            {
                await App.Logger.Log("[UPDATE] - Creating HTTP request to GitHub...");
                var httpRequest = (HttpWebRequest)WebRequest.Create(ReleasesUrl);
                httpRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                await App.Logger.Log("[UPDATE] - Waiting for response...");
                using (var webResponse = await httpRequest.GetResponseAsync())
                {
                    await App.Logger.Log("[UPDATE] - Reading response...");
                    var respStream = webResponse.GetResponseStream();

                    if (respStream == null)
                    {
                        await App.Logger.Log("[UPDATE] - Response is empty");
                        return null;
                    }
                    await App.Logger.Log("[UPDATE] - Response is not empty");
                    UpdateManifest? manifest = null;
                    using (var reader = new StreamReader(respStream))
                    {
                        await App.Logger.Log("[UPDATE] - Converting response to manifest");
                        var jsonData = reader.ReadToEnd();
                        manifest = JsonConvert.DeserializeObject<UpdateManifest>(jsonData);
                        await App.Logger.Log("[UPDATE] - Converted");
                    }
                    return manifest;
                }
            }
            catch (Exception ex)
            {
                await App.Logger.LogException("[UPDATE] - Could not check for updates!", ex: ex);
                return null;
            }
        }
        public struct UpdateManifest
        {
            public string name;
            public string tag_name;
            public string body;
            public Asset[] assets;
            [JsonObject("asset")]
            public class Asset
            {
                public string browser_download_url;
            }
        }
    }
}
