using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Updating
{
    public class GitHubUpdateManager : IUpdateManager
    {
        public const string ReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker/releases";
        public const string UpdaterReleasesUrl = "https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker.Updater/releases/latest";
        private static bool DownloadUpdater()
        {
            App.Logger.Log("[UPDATE] - Downloading updater");
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.UserAgent, "NPCMaker");
                    string content = client.DownloadString(UpdaterReleasesUrl);
                    var manifest = JsonConvert.DeserializeObject<UpdateManifest>(content);
                    var data = client.DownloadData(manifest.assets[0].browser_download_url);
                    using (FileStream fs = new FileStream(Path.Combine(AppConfig.Directory, "updater.exe"), FileMode.Create))
                    {
                        fs.Write(data, 0, data.Length);
                    }
                    return true;
                }
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
                System.Diagnostics.Process.Start(Path.Combine(AppConfig.Directory, "updater.exe"), $"\"{fileName}\"");
                Environment.Exit(0);
            }
            else
            {
                App.Logger.Log("[UPDATE] - Could not start update. Missing updater.");
            }
        }
        public async Task<UpdateAvailability> CheckForUpdates(bool checkForPrerelease)
        {
            try
            {
                await App.Logger.Log("[UPDATE] - Getting update manifest...");
                var manifest = await GetManifest(checkForPrerelease);
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
                    if (latestVers == App.Version)
                    {
                        await App.Logger.Log("[UPDATE] - User has latest version already");
                    }
                    else
                    {
                        await App.Logger.Log("[UPDATE] - User has newest version");
                    }
                    return UpdateAvailability.NOT_AVAILABLE;
                }
            }
            catch (Exception ex)
            {
                await App.Logger.LogException("[UPDATE] - Could not check for updates", ex: ex);
                return UpdateAvailability.ERROR;
            }
        }

        private async Task<UpdateManifest?> GetManifest(bool usePrerelease)
        {
            List<UpdateManifest> manifests = await GetManifests();
            UpdateManifest? latestManifest;
            await App.Logger.Log($"[UPDATE] - Use prerelease: {usePrerelease}");
            if (usePrerelease)
            {
                latestManifest = manifests.FirstOrDefault();
            }
            else
            {
                latestManifest = manifests.FirstOrDefault(d => d.prerelease == false);
            }
            if (latestManifest == null)
            {
                await App.Logger.Log("[UPDATE] - Could not get update manifest", Logging.ELogLevel.ERROR);
                return null;
            }
            else
            {
                var manifestPath = Path.Combine(AppConfig.Directory, "update.manifest");
                if (File.Exists(manifestPath))
                {
                    await App.Logger.Log("[UPDATE] - Deleting old update manifest...");
                    File.Delete(manifestPath);
                    await App.Logger.Log("[UPDATE] - Deleted");
                }
                try
                {
                    await App.Logger.Log("[UPDATE] - Saving update manifest...");
                    File.WriteAllText(manifestPath, JsonConvert.SerializeObject(latestManifest));
                    await App.Logger.Log("[UPDATE] - Saved");
                    return latestManifest;
                }
                catch (Exception ex)
                {
                    await App.Logger.LogException("[UPDATE] - Could not check for updates!", ex: ex);
                    return null;
                }
            }
        }
        public struct UpdateManifest
        {
            public string name;
            public string tag_name;
            public string body;
            public Asset[] assets;
            public bool prerelease;
            [JsonObject("asset")]
            public class Asset
            {
                public string browser_download_url;
            }
        }

        private async Task<List<UpdateManifest>> GetManifests()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.UserAgent, "NPCMaker");
                string response = await client.DownloadStringTaskAsync(ReleasesUrl);
                var manifests = JsonConvert.DeserializeObject<UpdateManifest[]>(response);
                return manifests.ToList();
            }
        }
    }
}
