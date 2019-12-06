using BowieD.Unturned.NPCMaker.Logging;
using System;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public class ShutdownCommand : Command
    {
        public override string Name => "shutdown";
        public override string Help => "Shutdowns application";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            App.Logger.Log("[ShutdownCommand] - Shutting down...");
            Environment.Exit(0);
        }
    }
}
