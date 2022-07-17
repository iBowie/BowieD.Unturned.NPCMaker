using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public sealed class RewardAchievement : Reward
    {
        public override RewardType Type => RewardType.Achievement;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Achievement"]} [{ID}]";
        [TextBoxOptions("Soulcrystal", "Mk2", "Boss_Magma", "Zweihander", "Kuwait_Squeek", "Kuwait_FelineFriends", "Kuwait_DinosaurJuice", "Kuwait_HomeDecor", "Kuwait_AlwaysWatching", "Kuwait_FamiliarFaces", "Elver_FinalBoss", "Festive2021_Power", "Festive2021_Snowman", "Festive2021_Delivery", "Festive2021_Cook", "Arid_Finale", "Arid_RPG", "Kuwait_EscapingOutlands", "Kuwait_DunemansPromise")]
        public string ID { get; set; }

        public override void Give(Simulation simulation) { }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToText();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteString(ID);
        }
    }
}
