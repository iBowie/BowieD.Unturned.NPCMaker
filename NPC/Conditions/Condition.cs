using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using BowieD.Unturned.NPCMaker.Localization;

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
    [XmlInclude(typeof(ConditionKillsPlayer))]
    [XmlInclude(typeof(ConditionKillsZombie))]
    [XmlInclude(typeof(ConditionPlayerLifeFood))]
    [XmlInclude(typeof(ConditionPlayerLifeHealth))]
    [XmlInclude(typeof(ConditionPlayerLifeVirus))]
    [XmlInclude(typeof(ConditionPlayerLifeWater))]
    [XmlInclude(typeof(ConditionSkillset))]
    [XmlInclude(typeof(ConditionTimeOfDay))]
    [XmlInclude(typeof(ConditionQuest))]
    [XmlInclude(typeof(ConditionCompareFlags))]
    [XmlInclude(typeof(ConditionHoliday))]
    public abstract class Condition : IHasDisplayName
    {
        [ConditionTooltip("Condition_Localization_Tooltip")]
        public string Localization;
        [XmlIgnore]
        public abstract Condition_Type Type { get; }
        [XmlIgnore]
        public abstract string DisplayName { get; }

        public IEnumerable<FrameworkElement> GetControls()
        {
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                string fieldName = field.Name;
                var fieldType = field.FieldType;
                string localizedName = LocUtil.LocalizeCondition($"Condition_{fieldName}");
                Grid borderContents = new Grid();
                Label l = new Label();
                l.Content = localizedName;
                var conditionTooltip = field.GetCustomAttribute<ConditionTooltipAttribute>();
                if (conditionTooltip != null)
                {
                    l.ToolTip = conditionTooltip.Text;
                }
                var conditionName = field.GetCustomAttribute<ConditionNameAttribute>();
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
                else if (fieldType == typeof(UInt16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt16.MaxValue,
                        Minimum = UInt16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                }
                else if (fieldType == typeof(Byte))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Byte.MaxValue,
                        Minimum = Byte.MinValue,
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
                            Content = LocUtil.LocalizeCondition($"Condition_{field.Name}_Enum_{(eValue.ToString())}")
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
                if (field.GetCustomAttribute<ConditionOptionalAttribute>() != null)
                    b.Opacity = 0.75;
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
                var skipFieldA = field.GetCustomAttribute<ConditionSkipFieldAttribute>();
                if ((skipLocalization && fieldName == "Localization") || skipFieldA != null)
                    continue;
                var fieldValue = field.GetValue(this);
                var noValueA = field.GetCustomAttribute<ConditionNoValueAttribute>();
                var optionalA = field.GetCustomAttribute<ConditionOptionalAttribute>();
                if (skipFieldA != null)
                    continue;
                if (noValueA != null)
                {
                    if (fieldValue.Equals(true))
                        fieldValue = "";
                    else
                        continue;
                }
                if (optionalA != null && optionalA.ConditionApplied(fieldValue))
                    continue;
                output.AppendLine($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_{fieldName}{(fieldValue.ToString().Length > 0 ? " " + fieldValue.ToString() : "")}");
            }
            return output.ToString();
        }
    }
    #region Attributes
    public class ConditionNameAttribute : Attribute
    {
        public string Text { get; private set; }
        public ConditionNameAttribute(string translationKey)
        {
            Text = LocUtil.LocalizeCondition(translationKey);
        }
    }
    public class ConditionTooltipAttribute : Attribute
    {
        public string Text { get; private set; }
        public ConditionTooltipAttribute(string translationKey)
        {
            Text = LocUtil.LocalizeCondition(translationKey);
        }
    }
    /// <summary>
    /// Apply to booleans
    /// </summary>
    public class ConditionNoValueAttribute : Attribute
    {

    }
    public class ConditionSkipFieldAttribute : Attribute
    {

    }
    public class ConditionOptionalAttribute : Attribute
    {
        public object defaultValue { get; private set; }
        public object skipValue { get; private set; }
        public ConditionOptionalAttribute(object defaultValue, object skipValue)
        {
            this.defaultValue = defaultValue;
            this.skipValue = skipValue;
        }
        public bool ConditionApplied(object currentValue)
        {
            if (skipValue.Equals(currentValue))
                return true;
            return false;
        }
    } 
#endregion
}
