using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        [ConditionSkipField]
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
                Label l = new Label
                {
                    Content = localizedName
                };
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
    }
}
