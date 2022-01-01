using System;
using System.Collections.Generic;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerVendorItemTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            var data = MainWindow.CurrentProject.data;

            return data.dialogueVendors.SelectMany(d => d.Items)
                .Concat(data.vendors.SelectMany(d => d.items));
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPC.VendorItem);

            yield return new ReplaceableProperty(nameof(NPC.VendorItem.id), type, FindReplaceFormats.ITEM_ID, (t) =>
            {
                return t is NPC.VendorItem vi && vi.type == NPC.ItemType.ITEM;
            });
            yield return new ReplaceableProperty(nameof(NPC.VendorItem.id), type, FindReplaceFormats.VEHICLE_ID, (t) =>
            {
                return t is NPC.VendorItem vi && vi.type == NPC.ItemType.VEHICLE;
            });
            yield return new ReplaceableProperty(nameof(NPC.VendorItem.spawnPointID), type, FindReplaceFormats.VEHICLE_SPAWNPOINT, (t) =>
            {
                return t is NPC.VendorItem vi && vi.type == NPC.ItemType.VEHICLE;
            });
        }
    }
}
