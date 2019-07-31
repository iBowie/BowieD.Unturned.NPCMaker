using System;
using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Vendor
    {
        public Vendor()
        {
            guid = Guid.NewGuid().ToString("N");
            title = new Dictionary<ELanguage, string>();
            description = new Dictionary<ELanguage, string>();
            shopSell = new List<VendorItem>();
            shopBuy = new List<VendorItem>();
        }
        public string guid;
        public ushort id;
        public Dictionary<ELanguage, string> title;
        public Dictionary<ELanguage, string> description;
        public List<VendorItem> shopSell;
        public List<VendorItem> shopBuy;

        public abstract class VendorItem
        {
            public ushort id;
            public uint price;
            public List<Condition.Condition> conditions;
        }
        public sealed class Item : VendorItem
        {
            public byte amount;
        }
        public sealed class Vehicle : VendorItem
        {
            public string spawnpoint;
        }
    }
}
