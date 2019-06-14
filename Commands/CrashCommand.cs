#if DEBUG
using System;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class CrashCommand : Command
    {
        public override string Name => "crash";
        public override string Help => "";
        public override string Syntax => "";
        public override void Execute(string[] args)
        {
            throw new Exception("User executed a crash!");
        }
    }
}
#endif