using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace BowieD.NPCMaker.Updater
{
    public sealed class GitHubUpdater : IUpdater
    {
        private const string url = @"https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker/releases/latest";
        public ECheckResult CheckUpdate()
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
                var respStream = httpRequest.GetResponse().GetResponseStream();
                if (respStream == null)
                    throw new Exception("null response");
                using (var reader = new StreamReader(respStream))
                {
                    var jsonData = reader.ReadToEnd();
                    var jsonObj = JObject.Parse(jsonData);
                    var latestVersionTag = jsonObj.GetValue("tag_name");

                    if (latestVersionTag == null)
                    {
                        throw new Exception("latest version not found.");
                    }
                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    Version latestVersion = Version.Parse(latestVersionTag.Value<string>());

                    if (currentVersion < latestVersion)
                    {
                        var additionalData = new JObject();
                        var body = jsonObj.GetValue("body")?.ToString() ?? "";
                        var assets = jsonObj.GetValue("assets").Children<JObject>();

                        additionalData.Add("changes", body);
                        additionalData.Add("download_url",
                            assets.First().GetValue("browser_download_url")?.ToString() ?? "null");
                        return ECheckResult.UpdateAvailable;
                    }
                    return ECheckResult.NoUpdates;
                }
            }
            catch
            {
                return ECheckResult.Error;
            }
        }
    }
}
