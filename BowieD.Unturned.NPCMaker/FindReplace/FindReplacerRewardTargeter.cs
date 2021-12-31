using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerRewardTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            var data = MainWindow.CurrentProject.data;

            return data.dialogues.SelectMany(d => d.Messages.SelectMany(k => k.rewards))
                .Concat(data.dialogues.SelectMany(d => d.Responses.SelectMany(k => k.rewards)))
                .Concat(data.dialogueVendors.SelectMany(d => d.Items.SelectMany(k => k.rewards)))
                .Concat(data.vendors.SelectMany(d => d.items.SelectMany(k => k.rewards)))
                .Concat(data.quests.SelectMany(d => d.rewards));
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            yield return new ReplaceableProperty(nameof(RewardItem.Sight), typeof(RewardItem), FindReplaceFormats.ITEM_OPTIONAL_ID);
            yield return new ReplaceableProperty(nameof(RewardItem.Grip), typeof(RewardItem), FindReplaceFormats.ITEM_OPTIONAL_ID);
            yield return new ReplaceableProperty(nameof(RewardItem.Magazine), typeof(RewardItem), FindReplaceFormats.ITEM_OPTIONAL_ID);
            yield return new ReplaceableProperty(nameof(RewardItem.Barrel), typeof(RewardItem), FindReplaceFormats.ITEM_OPTIONAL_ID);
            yield return new ReplaceableProperty(nameof(RewardItem.Tactical), typeof(RewardItem), FindReplaceFormats.ITEM_OPTIONAL_ID);

            yield return new ReplaceableProperty(nameof(RewardVehicle.ID), typeof(RewardVehicle), FindReplaceFormats.VEHICLE_ID);
        }
    }
}
