using System;
using System.Net;
using System.Threading;

namespace BowieD.Unturned.NPCMaker.Updater
{
    class Program
    {
        const string url = "https://raw.githubusercontent.com/iBowie/publicfiles/master/BowieD.Unturned.NPCMaker.exe";

        static void Main(string[] args)
        {
            for (int k = 0; k < 10; k++)
            {
                Console.WriteLine($"Try #{k} to update.");
                try
                {
                    Console.WriteLine("Updating...");
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFile(url, AppDomain.CurrentDomain.BaseDirectory + args[0]);
                    }
                    Console.WriteLine("Updated! Closing updater, opening app...");
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + args[0]);
                    return;
                }
                catch (Exception ex) { Console.WriteLine($"Try {k} failed. Reason: {ex.Message}"); Thread.Sleep(1000); }
            }
            Console.WriteLine($"Update failed. Press any key to close.");
            Console.ReadKey();
        }
    }
}
