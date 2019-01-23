using System;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlInclude(typeof(Rewards.Experience))]
    [XmlInclude(typeof(Rewards.Flag_Bool))]
    [XmlInclude(typeof(Rewards.Flag_Math))]
    [XmlInclude(typeof(Rewards.Flag_Short))]
    [XmlInclude(typeof(Rewards.Flag_Short_Random))]
    [XmlInclude(typeof(Rewards.Item))]
    [XmlInclude(typeof(Rewards.Item_Random))]
    [XmlInclude(typeof(Rewards.Quest))]
    [XmlInclude(typeof(Rewards.Reputation))]
    [XmlInclude(typeof(Rewards.Teleport))]
    [XmlInclude(typeof(Rewards.Vehicle))]
    public class Reward
    {
        public RewardType Type { get; set; }
        public string Localization { get; set; }

        public virtual string GetFilePresentation(string prefix, int prefixIndex, int rewardIndex)
        {
            throw new NotImplementedException();
        }
    }
}
