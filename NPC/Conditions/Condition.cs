using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [XmlInclude(typeof(ConditionExperience))]
    [XmlInclude(typeof(ConditionReputation))]
    [XmlInclude(typeof(ConditionFlagBool))]
    [XmlInclude(typeof(ConditionFlagShort))]
    [XmlInclude(typeof(ConditionItem))]
    [XmlInclude(typeof(ConditionKillsAnimal))]
    [XmlInclude(typeof(ConditionKillsHorde))]
    [XmlInclude(typeof(ConditionKillsObject))]
    [XmlInclude(typeof(ConditionKillsPlayers))]
    [XmlInclude(typeof(ConditionKillsZombie))]
    [XmlInclude(typeof(ConditionPlayerLifeFood))]
    [XmlInclude(typeof(ConditionPlayerLifeHealth))]
    [XmlInclude(typeof(ConditionPlayerLifeVirus))]
    [XmlInclude(typeof(ConditionPlayerLifeWater))]
    [XmlInclude(typeof(ConditionSkillset))]
    [XmlInclude(typeof(ConditionTimeOfDay))]
    [XmlInclude(typeof(ConditionQuest))]
    public abstract class Condition : IHasDisplayName
    {
        [ConditionTooltip("ConditionLocalization_Tooltip")]
        public string Localization;
        public abstract Condition_Type Type { get; }

        public abstract string DisplayName { get; }

        public IEnumerable<FrameworkElement> GetControls()
        {
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                string fieldName = field.Name;
                var fieldType = field.FieldType;
                string localizedName = MainWindow.Localize($"Condition{fieldName}");
                Grid borderContents = new Grid();
                Label l = new Label();
                l.Content = localizedName;
                var conditionTooltip = field.GetCustomAttribute<ConditionTooltip>();
                if (conditionTooltip != null)
                {
                    l.ToolTip = conditionTooltip.Text;
                }
                var conditionName = field.GetCustomAttribute<ConditionName>();
                if (conditionName != null)
                {
                    l.Content = conditionName.Text;
                }
                borderContents.Children.Add(l);
                FrameworkElement valueControl = null;
                if (fieldType == typeof(UInt16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt16.MaxValue,
                        Minimum = UInt16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                }
                else if (fieldType == typeof(UInt32))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt32.MaxValue,
                        Minimum = UInt32.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                }
                else if (fieldType == typeof(Int32))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Int32.MaxValue,
                        Minimum = Int32.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                }
                else if (fieldType == typeof(Int16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Int16.MaxValue,
                        Minimum = Int16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                }
                else if (fieldType == typeof(string))
                {
                    valueControl = new TextBox()
                    {
                        MaxWidth = 100,
                        TextWrapping = TextWrapping.Wrap
                    };
                }
                else if (fieldType.IsEnum)
                {
                    var cBox = new ComboBox();
                    var values = Enum.GetValues(fieldType);
                    foreach (var eValue in values)
                    {
                        ComboBoxItem cbi = new ComboBoxItem
                        {
                            Tag = eValue,
                            Content = MainWindow.Localize($"Condition_{field.Name}_Enum_{(eValue.ToString())}")
                        };
                        cBox.Items.Add(cbi);
                    }
                    valueControl = cBox;
                }
                else if (fieldType == typeof(bool))
                {
                    valueControl = new CheckBox()
                    {
                        
                    };
                }
                valueControl.HorizontalAlignment = HorizontalAlignment.Right;
                valueControl.VerticalAlignment = VerticalAlignment.Center;
                borderContents.Children.Add(valueControl);
                valueControl.Tag = "variable::" + fieldName;
                Border b = new Border
                {
                    Child = borderContents
                };
                b.Margin = new Thickness(0, 5, 0, 5);
                yield return b;
            }
        }
        public static IEnumerable<Type> GetTypes()
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t != null && t.IsSealed && t.IsSubclassOf(typeof(Condition)))
                {
                    yield return t;
                }
            }
        }
        public string GetFullFilePresentation(string prefix, int prefixIndex, int conditionIndex, bool skipLocalization = true)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            StringBuilder output = new StringBuilder();
            output.AppendLine($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type {Type.ToString()}");
            foreach (var field in this.GetType().GetFields())
            {
                var fieldName = field.Name;
                if ((skipLocalization && fieldName == "Localization") || field.GetCustomAttribute<ConditionOptional>() != null)
                    continue;
                var fieldValue = field.GetValue(this);
                output.AppendLine($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_{fieldName} {fieldValue.ToString()}");
            }
            return output.ToString();
        }
    }

    public class ConditionName : Attribute
    {
        public string Text { get; private set; }
        public ConditionName(string translationKey)
        {
            Text = MainWindow.Localize(translationKey);
        }
    }
    public class ConditionTooltip : Attribute
    {
        public string Text { get; private set; }
        public ConditionTooltip(string translationKey)
        {
            Text = MainWindow.Localize(translationKey);
        }
    }
    public class ConditionNoValue : Attribute
    {

    }
    public class ConditionOptional : Attribute
    {

    }

    public sealed class ConditionExperience : Condition
    {
        public override Condition_Type Type => Condition_Type.Experience;
        public override string DisplayName
        {
            get
            {
                string outp = MainWindow.Localize("Condition_Type_ConditionExperience") + " ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += Value;
                return outp;
            }
        }

        public Logic_Type Logic;
        [ConditionName("ConditionAmount")]
        public uint Value;
        [ConditionName("ConditionReset_Experience_Title")]
        public bool Reset;
    }
    public sealed class ConditionReputation : Condition
    {
        public override Condition_Type Type => Condition_Type.Reputation;
        [ConditionName("ConditionAmount")]
        public int Value;
        public Logic_Type Logic;
        [ConditionName("ConditionReset_Reputation_Title")]
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                string outp = MainWindow.Localize("Condition_Type_ConditionReputation") + " ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += Value;
                return outp;
            }
        }
    }
    public sealed class ConditionFlagBool : Condition
    {
        public override Condition_Type Type => Condition_Type.Flag_Bool;
        public ushort ID;
        public bool Value;
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                return $"{MainWindow.Localize("Condition_Type_ConditionFlagBool")} [{ID}] = {Value}";
            }
        }
    }
    public sealed class ConditionQuest : Condition
    {
        public override Condition_Type Type => Condition_Type.Quest;
        public ushort ID;
        public Quest_Status Status;
        public Logic_Type Logic;
        [ConditionName("ConditionReset_Quest_Title")]
        [ConditionTooltip("ConditionReset_Quest_Tooltip")]
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                string outp = MainWindow.Localize("Condition_Type_ConditionQuest") + $" [{ID}] ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += MainWindow.Localize($"Condition_Status_Enum_{Status.ToString()}");
                return outp;
            }
        }
    }
    public sealed class ConditionKillsPlayers : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Player;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsPlayers")} >= {Value}";
            }
        }
        public ushort ID;
        [ConditionName("ConditionAmount")]
        public short Value;
        public bool Reset;
    }
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsAnimal")} ({Animal}) >= {Value}";
            }
        }
        public ushort ID; 
        public ushort Animal;
        public short Value;
        public bool Reset;
    }
    public sealed class ConditionKillsHorde : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Horde;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsHorde")} ({Nav}) >= {Value}";
            }
        }
        public ushort ID;
        public short Value;
        public byte Nav;
        public bool Reset;
    }
    public sealed class ConditionKillsZombie : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Zombie;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {MainWindow.Localize("Condition_Type_ConditionKillsZombie")} : {MainWindow.Localize($"Condition_Zombie_Enum_{Zombie.ToString()}")} ({Nav}) >= {Value} {(Spawn ? )}";
            }
        }
        public ushort ID;
        public short Value;
        public int Spawn_Quantity;
        public bool Spawn;
        public byte Nav;
        public bool Reset;
        public Zombie_Type Zombie;
    }
}
