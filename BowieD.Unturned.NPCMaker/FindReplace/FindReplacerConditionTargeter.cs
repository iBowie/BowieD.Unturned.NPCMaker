using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerConditionTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            var data = MainWindow.CurrentProject.data;

            return data.characters.SelectMany(d => d.visibilityConditions)
                .Concat(data.dialogues.SelectMany(d => d.Messages.SelectMany(k => k.conditions)))
                .Concat(data.dialogues.SelectMany(d => d.Responses.SelectMany(k => k.conditions)))
                .Concat(data.dialogueVendors.SelectMany(d => d.Items.SelectMany(k => k.conditions)))
                .Concat(data.vendors.SelectMany(d => d.items.SelectMany(k => k.conditions)))
                .Concat(data.quests.SelectMany(d => d.conditions));
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            return Array.Empty<ReplaceableProperty>();
        }
    }
}
