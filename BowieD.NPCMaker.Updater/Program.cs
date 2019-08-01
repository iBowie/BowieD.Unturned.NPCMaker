using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace BowieD.NPCMaker.Updater
{
    class Program
    {
        private const string url = @"https://api.github.com/repos/iBowie/BowieD.Unturned.NPCMaker/releases/latest";
        public static UpdateManifest? GetUpdateManifest()
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
            var respStream = httpRequest.GetResponse().GetResponseStream();
            if (respStream == null)
                return null;
            UpdateManifest result = new UpdateManifest();
            using (var reader = new StreamReader(respStream))
            {
                var jsonData = reader.ReadToEnd();
                var jsonObj = JObject.Parse(jsonData);
                var latestVersionTag = jsonObj.GetValue("tag_name");

                if (latestVersionTag == null)
                    return result;
                result.newVersion = latestVersionTag.ToString();
                var assets = jsonObj.GetValue("assets").Children<JObject>();
                string downloadUrl = assets.First().GetValue("browser_download_url")?.ToString() ?? "null";
                if (downloadUrl == "null")
                    return result;
                result.downloadUrl = downloadUrl;
            }
            return result;
        }
        static void Main(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                WriteLine($"ILLEGAL ARGUMENTS. CLOSING...", ConsoleColor.Red);
                Thread.Sleep(3000);
                return;
            }
            for (int k = 0; k < 10; k++)
            {
                WriteLine($"Try #{k}", ConsoleColor.White);
                try
                {
                    WriteLine($"Getting manifest...", ConsoleColor.White);
                    var manifest = GetUpdateManifest();
                    if (manifest == null)
                        WriteLine($"FAILED: Could not get anything.", ConsoleColor.Red);
                    else if (manifest?.newVersion == null)
                        WriteLine($"FAILED: Could not get version info.", ConsoleColor.Red);
                    else if (manifest?.downloadUrl == null)
                        WriteLine($"FAILED: Could not get download URL.", ConsoleColor.Red);
                    else
                    {
                        WriteLine($"SUCCESS: Version info and download URL obtained.", ConsoleColor.Green);
                        if (File.Exists(args[0]))
                        {
                            WriteLine($"Deleting old version...", ConsoleColor.White);
                            File.Delete(args[0]);
                            WriteLine($"Deleted.", ConsoleColor.White);
                        }
                        WriteLine($"Downloading new version...", ConsoleColor.White);
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(manifest?.downloadUrl, args[0]);
                        }
                        WriteLine($"Downloaded!", ConsoleColor.White);
                        WriteLine($"Launching in 3...", ConsoleColor.White);
                        Thread.Sleep(1000);
                        WriteLine($"Launching in 2...", ConsoleColor.White);
                        Thread.Sleep(1000);
                        WriteLine($"Launching in 1...", ConsoleColor.White);
                        Thread.Sleep(1000);
                        System.Diagnostics.Process.Start(args[0]);
                        return;
                    }
                }
                catch
                {

                }
            }
            Console.WriteLine($"Update failed. Try again later.");
            Console.WriteLine($"Press any key to close.");
            Console.ReadKey(true);
        }
        static void WriteLine(object text, ConsoleColor color)
        {
            var oldClr = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldClr;
        }
    }
    public struct UpdateManifest
    {
        public string newVersion;
        public string downloadUrl;
    }
}
