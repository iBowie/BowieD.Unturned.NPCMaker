using BowieD.Unturned.NPCMaker.BetterForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public virtual void Init(Universal_RewardEditor ure)
        {
            throw new NotImplementedException();
        }
        public virtual void Init(Universal_RewardEditor ure, Reward start)
        {
            throw new NotImplementedException();
        }
        public virtual int Elements => 0;
        public virtual T Parse<T>(object[] input) where T : Reward
        {
            throw new NotImplementedException();
        }
        public virtual string GetFilePresentation(string prefix, int prefixIndex, int rewardIndex)
        {
            throw new NotImplementedException();
        }
        public static Type GetByType(RewardType type) => RewardObjects.First(d => d.Type == type).GetType();
        public static HashSet<Reward> RewardObjects
        {
            get
            {
                if (rewObjs == null)
                {
                    rewObjs = new HashSet<Reward>();
                    var objs = RewardTypes.Select(d => (Reward)Activator.CreateInstance(d)).Where(d => d is Reward).ToList();
                    objs.ForEach(d => rewObjs.Add(d));
                }
                return rewObjs;
            }
        }
        public static HashSet<Type> RewardTypes
        {
            get
            {
                if (rewTypes == null)
                {
                    var classes = Assembly.GetExecutingAssembly().GetTypes().Where(d => d.IsClass && d.Namespace == "BowieD.Unturned.NPCMaker.NPC.Rewards").ToList();
                    rewTypes = new HashSet<Type>();
                    classes.ForEach(d => rewTypes.Add(d));
                }
                return rewTypes;
            }
        }
        private static HashSet<Type> rewTypes;
        private static HashSet<Reward> rewObjs;
    }
}
