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
            yield return new ReplaceableProperty(nameof(NPCResponse.openVendorId), type, FindReplaceFormats.VENDOR_ID);
            yield return new ReplaceableProperty(nameof(NPCResponse.openQuestId), type, FindReplaceFormats.QUEST_ID);
        }
    }
}
