#if DEBUG
using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Runtime;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class GCCommand : Command
    {
        public override string Name => "gc";
        public override string Help => "Interact with Garbage Collector";
        public override string Syntax => "<run/stop/resume>";
        private GCLatencyMode oldMode = GCLatencyMode.Batch;
        public override void Execute(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "run":
                        GC.Collect();
                        App.Logger.LogInfo("[GCCommand] - GC Forced");
                        break;
                    case "stop" when GCSettings.LatencyMode != GCLatencyMode.LowLatency:
                        oldMode = GCSettings.LatencyMode;
                        GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                        App.Logger.LogInfo("[GCCommand] - GC Paused");
                        break;
                    case "resume" when GCSettings.LatencyMode == GCLatencyMode.LowLatency:
                        GCSettings.LatencyMode = oldMode;
                        App.Logger.LogInfo("[GCCommand] - GC Resumed");
                        break;
                }
            }
        }
    }
}
#endif