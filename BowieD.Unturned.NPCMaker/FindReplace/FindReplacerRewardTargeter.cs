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

            yield return new ReplaceableProperty(nameof(RewardQuest.ID), typeof(RewardQuest), FindReplaceFormats.QUEST_ID);

            yield return new ReplaceableProperty(nameof(RewardVehicle.Spawnpoint), typeof(RewardVehicle), FindReplaceFormats.VEHICLE_SPAWNPOINT);

            yield return new ReplaceableProperty(nameof(RewardFlagBool.ID), typeof(RewardFlagBool), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(RewardFlagMath.A_ID), typeof(RewardFlagMath), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(RewardFlagMath.B_ID), typeof(RewardFlagMath), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(RewardFlagShort.ID), typeof(RewardFlagShort), FindReplaceFormats.FLAG_ID);
            yield return new ReplaceableProperty(nameof(RewardFlagShortRandom.ID), typeof(RewardFlagShortRandom), FindReplaceFormats.FLAG_ID);

            yield return new ReplaceableProperty(nameof(RewardCurrency.GUID), typeof(RewardCurrency), FindReplaceFormats.CURRENCY_GUID);
        }
    }
}
