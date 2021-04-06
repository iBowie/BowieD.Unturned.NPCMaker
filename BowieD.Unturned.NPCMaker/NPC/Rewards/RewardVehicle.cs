using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardVehicle : Reward
    {
        public override RewardType Type => RewardType.Vehicle;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(LocalizationManager.Current.Reward["Type_Vehicle"]);


                if (GameAssetManager.TryGetAsset<GameVehicleAsset>(ID, out var asset))
                {
                    sb.Append($" {asset.name} -> [{Spawnpoint}]");
                }
                else
                {
                    sb.Append($" {ID} -> [{Spawnpoint}]");
                }

                return sb.ToString();
            }
        }

        [AssetPicker(typeof(GameVehicleAsset), "Control_SelectAsset_Vehicle", MahApps.Metro.IconPacks.PackIconMaterialKind.Car)]
        public ushort ID { get; set; }
        public string Spawnpoint { get; set; }

        public override void Give(Simulation simulation) { }
        public override string FormatReward(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Reward_Vehicle");
            }
            return string.Format(text, ID);
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Spawnpoint = node["Spawnpoint"].InnerText;
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Spawnpoint", node).WriteString(Spawnpoint);
        }
    }
}
