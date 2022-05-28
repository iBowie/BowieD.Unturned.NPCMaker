using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration.Devkit;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    [Configuration.SkillLock(Configuration.ESkillLevel.Advanced)]
    public sealed class RewardTeleport : Reward
    {
        public override RewardType Type => RewardType.Teleport;
        public override string UIText => $"{LocalizationManager.Current.Reward["Type_Teleport"]} [{Spawnpoint}]";

        [AssetPicker(typeof(Spawnpoint), "Control_SelectAsset_DKSpawnpoint", MahApps.Metro.IconPacks.PackIconMaterialKind.MapMarker)]
        public string Spawnpoint { get; set; }

        public override void Give(Simulation simulation) { }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Spawnpoint = node["Spawnpoint"].ToText();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Spawnpoint", node).WriteString(Spawnpoint);
        }
    }
}
