using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    [XmlInclude(typeof(Conditions.Kills_Players_Cond))]
    [XmlInclude(typeof(Conditions.Kills_Object_Cond))]
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
        public virtual int Elements => 0;

        public Condition()
        {
            Type = Condition_Type.None;
        }

        public virtual void Init(Universal_ConditionEditor uce)
        {
            throw new NotImplementedException();
        }
        public virtual void Init(Universal_ConditionEditor uce, Condition start)
        {
            throw new NotImplementedException();
        }

        public virtual T Parse<T>(object[] input) where T : Condition
        {
            throw new NotImplementedException();
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

        public static Type GetByType(Condition_Type type) => ConditionObjects.First(d => d.Type == type).GetType();
        public static HashSet<Condition> ConditionObjects
        {
            get
            {
                if (condObjs == null)
                {
                    condObjs = new HashSet<Condition>();
                    var objs = ConditionTypes.Select(d => (Condition)Activator.CreateInstance(d)).Where(d => d is Condition).ToList();
                    objs.ForEach(d => condObjs.Add(d));
                }
                return condObjs;
            }
        }
        public static HashSet<Type> ConditionTypes
        {
            get
            {
                if (condTypes == null)
                {
                    var classes = Assembly.GetExecutingAssembly().GetTypes().Where(d => d.IsClass && d.Namespace == "BowieD.Unturned.NPCMaker.NPC.Conditions").ToList();
                    condTypes = new HashSet<Type>();
                    classes.ForEach(d => condTypes.Add(d));
                }
                return condTypes;
            }
        }
        private static HashSet<Type> condTypes;
        private static HashSet<Condition> condObjs;
    }
}
