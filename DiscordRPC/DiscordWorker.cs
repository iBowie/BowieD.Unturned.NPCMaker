using DiscordRPC;
using DiscordRPC.Logging;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.DiscordRPC
{
    public class DiscordWorker
    {
        public DiscordRpcClient client;
        public int ticksDelay;

        public DiscordWorker(int delay)
        {
            ticksDelay = delay;
        }

        public void Initialize() // Run when app starts
        {
            client = new DiscordRpcClient("528291563181178900")
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };
            client.OnReady += Client_OnReady;
            client.OnPresenceUpdate += Client_OnPresenceUpdate;
            client.Initialize();
            Update();
        }

        public void SendPresence(RichPresence rich)
        {
            if (rich.Assets == null)
                rich.Assets = new Assets();
            rich.Assets.LargeImageText = $"NPC Maker for Unturned by BowieD. Version: {MainWindow.Version}";
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
        }

        private void Client_OnPresenceUpdate(object sender, global::DiscordRPC.Message.PresenceMessage args)
        {

        }

        private void Client_OnReady(object sender, global::DiscordRPC.Message.ReadyMessage args)
        {
            Logging.Logger.Log("Discord Rich Presence started!", Logging.Log_Level.Normal);
            MainWindow.Instance.DoNotification(MainWindow.Localize("menu_Discord_Start"));
        }
    }
}
