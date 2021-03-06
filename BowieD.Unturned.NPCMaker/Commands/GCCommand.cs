﻿#if DEBUG
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
                        App.Logger.Log("[GCCommand] - GC Forced");
                        break;
                    case "stop" when GCSettings.LatencyMode != GCLatencyMode.LowLatency:
                        oldMode = GCSettings.LatencyMode;
                        GCSettings.LatencyMode = GCLatencyMode.LowLatency;
                        App.Logger.Log("[GCCommand] - GC Paused");
                        break;
                    case "resume" when GCSettings.LatencyMode == GCLatencyMode.LowLatency:
                        GCSettings.LatencyMode = oldMode;
                        App.Logger.Log("[GCCommand] - GC Resumed");
                        break;
                }
            }
        }
    }
}
#endif