using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerDialogueResponseTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.dialogues.SelectMany(d => d.Responses);
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCResponse);

            yield return new ReplaceableProperty(nameof(NPCResponse.openDialogueId), type, FindReplaceFormats.DIALOGUE_ID);
        }
    }
}
