using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Vendor
    {
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
