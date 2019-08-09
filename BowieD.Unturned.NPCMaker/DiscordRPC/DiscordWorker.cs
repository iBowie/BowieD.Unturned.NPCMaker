using BowieD.Unturned.NPCMaker.Logging;
using DiscordRPC;
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
                rich = new RichPresence();
                rich.State = "User hides details";
                rich.Details = "Editing NPC";
            }
            if (rich.Assets == null)
                rich.Assets = new Assets();
            rich.Assets.LargeImageText = $"NPC Maker for Unturned by BowieD. Version: {App.Version}";
            rich.Assets.LargeImageKey = "mainimage_outline";
            if (client.IsInitialized)
                client.SetPresence(rich);
        }

        public void SendPresence(string details, string state)
        {
            SendPresence(new RichPresence() { Details = details, State = state });
        }

        public async void Update()
        {
            client.Invoke();
            await Task.Delay(ticksDelay);
            if (!client.Disposed)
                Update();
        }

        public void Deinitialize()
        {
            client.Dispose();
            App.Logger.LogInfo("[DISCORD] - Rich Presence client deinitialized!");
        }

        private void Client_OnPresenceUpdate(object sender, global::DiscordRPC.Message.PresenceMessage args)
        {

        }

        private void Client_OnReady(object sender, global::DiscordRPC.Message.ReadyMessage args)
        {
            App.Logger.LogInfo("[DISCORD] - Rich Presence started!");
        }
    }
}
