using BowieD.Unturned.NPCMaker.NPC.Conditions;
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
            yield return new ReplaceableProperty(nameof(ConditionQuest.ID), typeof(ConditionQuest), FindReplaceFormats.QUEST_ID);

            yield return new ReplaceableProperty(nameof(ConditionCompareFlags.A_ID), typeof(ConditionCompareFlags), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionCompareFlags.B_ID), typeof(ConditionCompareFlags), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionFlagBool.ID), typeof(ConditionFlagBool), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionFlagShort.ID), typeof(ConditionFlagShort), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsAnimal.ID), typeof(ConditionKillsAnimal), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsHorde.ID), typeof(ConditionKillsHorde), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsObject.ID), typeof(ConditionKillsObject), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsPlayer.ID), typeof(ConditionKillsPlayer), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsTree.ID), typeof(ConditionKillsTree), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(ConditionKillsZombie.ID), typeof(ConditionKillsZombie), FindReplaceFormats.FLAG_ID);
            
            yield return new ReplaceableProperty(nameof(ConditionCurrency.GUID), typeof(ConditionCurrency), FindReplaceFormats.CURRENCY_GUID);
        }
    }
}
