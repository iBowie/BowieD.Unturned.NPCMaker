using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionKillsTree : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }
        [AssetPicker(typeof(GameResourceAsset), "Control_SelectAsset_Resource", MahApps.Metro.IconPacks.PackIconMaterialKind.Tree)]
        public string Tree { get; set; }
        public override Condition_Type Type => Condition_Type.Kills_Tree;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (Guid.TryParse(Tree, out var treeGuid) && GameAssetManager.TryGetAsset<GameResourceAsset>(treeGuid, out var resourceAsset))
                {
                    sb.Append($"[{ID}] {resourceAsset.name} x{Value}");
                }
                else
                {
                    sb.Append($"[{ID}] {Tree} x{Value}");
                }

                return sb.ToString();
            }
        }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out short flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_TreeKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            string arg;

            if (Guid.TryParse(Tree, out var treeGUID) && GameAssetManager.TryGetAsset<GameResourceAsset>(treeGUID, out var treeAsset))
            {
                arg = treeAsset.name;
            }
            else
            {
                arg = "?";
            }

            return string.Format(text, value, Value, arg);
        }

        public override void Load(System.Xml.XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Value = node["Value"].ToInt16();
            Tree = node["Tree"].ToText();
        }

        public override void Save(System.Xml.XmlDocument document, System.Xml.XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Value", node).WriteInt16(Value);
            document.CreateNodeC("Tree", node).WriteString(Tree);
        }
    }
}
