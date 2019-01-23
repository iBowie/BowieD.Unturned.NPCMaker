using System;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [XmlInclude(typeof(Conditions.Experience_Cond))]
    [XmlInclude(typeof(Conditions.Flag_Bool_Cond))]
    [XmlInclude(typeof(Conditions.Flag_Short_Cond))]
    [XmlInclude(typeof(Conditions.Item_Cond))]
    [XmlInclude(typeof(Conditions.Kills_Animal_Cond))]
    [XmlInclude(typeof(Conditions.Kills_Horde_Cond))]
    [XmlInclude(typeof(Conditions.Kills_Zombie_Cond))]
    [XmlInclude(typeof(Conditions.Quest_Cond))]
    [XmlInclude(typeof(Conditions.Reputation_Cond))]
    [XmlInclude(typeof(Conditions.Skillset_Cond))]
    [XmlInclude(typeof(Conditions.Time_Of_Day_Cond))]
    [XmlInclude(typeof(Conditions.Player_Life_Food_Cond))]
    [XmlInclude(typeof(Conditions.Player_Life_Water_Cond))]
    [XmlInclude(typeof(Conditions.Player_Life_Health_Cond))]
    [XmlInclude(typeof(Conditions.Player_Life_Virus_Cond))]
    public class Condition
    {
        public string Localization { get; set; }
        public bool Reset { get; set; }
        public Condition_Type Type { get; set; }

        public Condition()
        {
            Type = Condition_Type.None;
        }

        public virtual string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            throw new NotImplementedException();
        }
        public string GetFullFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (Reset)
                return GetFilePresentation(prefix, prefixIndex, conditionIndex) + $"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Reset";
            else
                return GetFilePresentation(prefix, prefixIndex, conditionIndex);
        }

        public override string ToString()
        {
            return Localization != null && Localization.Length > 0 ? Localization : Type.ToString();
        }
    }
}
