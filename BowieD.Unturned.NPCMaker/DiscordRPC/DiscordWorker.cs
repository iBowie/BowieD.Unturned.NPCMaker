using DiscordRPC;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.DiscordRPC
{
    public class DiscordManager
    {
        private DiscordRpcClient client;
        public int ticksDelay;
        public bool descriptive { get; set; }

        public DiscordManager(int delay)
        {
            ticksDelay = delay;
        }

        public void Initialize() // Run when app starts
        {
            client = new DiscordRpcClient("528291563181178900")
            {

            };
            client.OnReady += Client_OnReady;
            client.OnPresenceUpdate += Client_OnPresenceUpdate;
            client.Initialize();
            Update();
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

        public async void Update()
        {
            client.Invoke();
            await Task.Delay(ticksDelay);
            if (!client.Disposed)
            {
                Update();
            }
        }

        public void Deinitialize()
        {
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
