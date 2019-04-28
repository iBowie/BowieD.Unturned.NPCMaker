using BowieD.Unturned.NPCMaker.BetterForms;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
    public class Condition : IHasDisplayName
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
        public string DisplayName
        {
            get
            {
                if (Localization?.Length > 0)
                    return $"{Localization}";
                else
                {
                    StringBuilder str = new StringBuilder();
                    str.Append(MainWindow.Localize("Condition_" + this.Type.ToString()));
                    if (this is IHasLogic hasLogic)
                    {
                        switch (hasLogic.Logic)
                        {
                            case Logic_Type.Equal:
                                str.Append(" = ");
                                break;
                            case Logic_Type.Greater_Than:
                                str.Append(" > ");
                                break;
                            case Logic_Type.Greater_Than_Or_Equal_To:
                                str.Append(" >= ");
                                break;
                            case Logic_Type.Less_Than:
                                str.Append(" < ");
                                break;
                            case Logic_Type.Less_Than_Or_Equal_To:
                                str.Append(" <= ");
                                break;
                            case Logic_Type.Not_Equal:
                                str.Append(" != ");
                                break;
                        }
                    }
                    switch (this)
                    {
                        case IHasValue<uint> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<int> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<short> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<ushort> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<long> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<ulong> val:
                            str.Append($" {val.Value} ");
                            break;
                        case IHasValue<ESkillset> val:
                            str.Append($" {MainWindow.Localize($"Skillset_{val.Value.ToString()}")} ");
                            break;
                        case IHasValue<bool> val:
                            str.Append($" {(val.Value ? "TRUE" : "FALSE")} ");
                            break;
                    }
                    if (this is Quest_Cond questCond)
                    {
                        str.Append($" {MainWindow.Localize("QuestStatus_" + questCond.Status.ToString())} ");
                    }
                    if (this is Item_Cond itemCond)
                    {
                        str.Append($" {itemCond.Id} x{itemCond.Amount}");
                    }
                    return str.ToString();
                }
            }
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

        public interface IHasLogic
        {
            Logic_Type Logic { get; set; }
        }
        public interface IHasValue<T>
        {
            T Value { get; set; }
        }
    }
}
