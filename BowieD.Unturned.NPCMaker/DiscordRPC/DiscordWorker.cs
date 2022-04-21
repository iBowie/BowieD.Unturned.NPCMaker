using DiscordRPC;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker.DiscordRPC
{
    public class DiscordManager
    {
        private DiscordRpcClient client;
        private DispatcherTimer _timer;
        public readonly int ticksDelay;
        public bool descriptive { get; set; }

        public DiscordManager(int delay)
        {
            ticksDelay = delay;

            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(delay),
            };
            _timer.Tick += Update;
        }

        public void Initialize() // Run when app starts
        {
            client = new DiscordRpcClient("528291563181178900")
            {

            };
            client.OnReady += Client_OnReady;
            client.OnPresenceUpdate += Client_OnPresenceUpdate;
            client.Initialize();
            _timer.Start();
        }

        public void SendPresence(RichPresence rich)
        {
            if (!descriptive)
            {
                rich = new RichPresence
                {
                    State = "User hides details",
                    Details = "Editing NPC"
                };
            }
            if (rich.Assets == null)
            {
                rich.Assets = new Assets();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"NPC Maker for Unturned by BowieD. Version: {App.Version}");

#if DEBUG
            sb.Append(" DEBUG");
#elif PREVIEW
            sb.Append(" PREVIEW");
#else
#endif
            rich.Assets.LargeImageText = sb.ToString();

            rich.Assets.LargeImageKey = "mainimage_outline";
            if (client.IsInitialized)
            {
                client.SetPresence(rich);
            }
        }

        private void Update(object sender, EventArgs e)
        {
            client.Invoke();
        }

        public void Deinitialize()
        {
            _timer.Stop();
            client.Dispose();
            App.Logger.Log("[DISCORD] - Rich Presence client deinitialized!");
        }

        private void Client_OnPresenceUpdate(object sender, global::DiscordRPC.Message.PresenceMessage args)
        {

        }

        private void Client_OnReady(object sender, global::DiscordRPC.Message.ReadyMessage args)
        {
            App.Logger.Log("[DISCORD] - Rich Presence started!");
        }
    }
}
