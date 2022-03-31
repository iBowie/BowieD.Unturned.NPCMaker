using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerDialogueMessageTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.dialogues.SelectMany(d => d.Messages);
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCMessage);

            yield return new ReplaceableProperty(nameof(NPCMessage.prev), type, FindReplaceFormats.DIALOGUE_ID);
        }
    }
}
