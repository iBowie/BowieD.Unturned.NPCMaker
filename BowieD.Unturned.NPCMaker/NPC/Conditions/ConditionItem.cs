using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionItem : Condition, IUIL_Icon
    {
        public override Condition_Type Type => Condition_Type.Item;
        [AssetPicker(typeof(GameItemAsset), "Control_SelectAsset_Item", MahApps.Metro.IconPacks.PackIconMaterialKind.Archive)]
        public ushort ID { get; set; }
        public ushort Amount { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(LocalizationManager.Current.Condition[$"Type_Item"] + " ");
                if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
                {
                    sb.Append($"{asset.name} x{Amount}");
                }
                else
                {
                    sb.Append($"{ID} x{Amount}");
                }
                return sb.ToString();
            }
        }

        public override bool Check(Simulation simulation)
        {
            System.Collections.Generic.List<Simulation.Item> items = simulation.Items.Where(d => d.ID == ID).ToList();

            return SimulationTool.Compare(items.Count, Amount, Logic_Type.Greater_Than_Or_Equal_To);
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                foreach (Simulation.Item i in simulation.Items.Where(d => d.ID == ID).Take(Amount).ToList())
                {
                    simulation.Items.Remove(i);
                }
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Condition_Item");
            }

            System.Collections.Generic.IEnumerable<Simulation.Item> found = simulation.Items.Where(d => d.ID == ID);

            string idOrName;

            if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var gameAsset))
            {
                idOrName = gameAsset.name;
            }
            else
            {
                idOrName = ID.ToString();
            }

            return string.Format(text, found.Count(), Amount, idOrName);
        }

        public bool UpdateIcon(out BitmapImage image)
        {
            if (ID > 0 && GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
            {
                image = ThumbnailManager.CreateThumbnail(asset.ImagePath);
                return true;
            }
            else
            {
                image = default;
                return false;
            }
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Amount = node["Amount"].ToUInt16();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Amount", node).WriteUInt16(Amount);
        }
    }
}
