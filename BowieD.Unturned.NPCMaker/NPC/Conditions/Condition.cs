using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.XAML;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        [ConditionSkipField]
        public string Localization { get; set; }
        [XmlIgnore]
        public abstract Condition_Type Type { get; }
        [XmlIgnore]
        public abstract string DisplayName { get; }

        public IEnumerable<FrameworkElement> GetControls()
        {
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanRead)
                    continue;
                string propName = prop.Name;
                var propType = prop.PropertyType;
                string localizedName = LocalizationManager.Current.Condition[$"{Type}_{propName}"];
                Grid borderContents = new Grid();
                Label l = new Label
                {
                    Content = localizedName
                };
                var conditionTooltip = prop.GetCustomAttribute<ConditionTooltipAttribute>();
                if (conditionTooltip != null)
                {
                    l.ToolTip = conditionTooltip.Text;
                }
                borderContents.Children.Add(l);
                FrameworkElement valueControl = null;
                if (propType == typeof(UInt16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt16.MaxValue,
                        Minimum = UInt16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(UInt32))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt32.MaxValue,
                        Minimum = UInt32.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(Int32))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Int32.MaxValue,
                        Minimum = Int32.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(Int16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Int16.MaxValue,
                        Minimum = Int16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(UInt16))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = UInt16.MaxValue,
                        Minimum = UInt16.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(Byte))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = Byte.MaxValue,
                        Minimum = Byte.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(string))
                {
                    valueControl = new TextBox()
                    {
                        MaxWidth = 100,
                        TextWrapping = TextWrapping.Wrap
                    };
                    (valueControl as TextBox).SetBinding(TextBox.TextProperty, propName);
                }
                else if (propType.IsEnum)
                {
                    var cBox = new ComboBox();
                    var values = Enum.GetValues(propType);
                    foreach (var eValue in values)
                    {
                        cBox.Items.Add(LocalizationManager.Current.Condition[$"{Type}_{prop.Name}_{eValue.ToString()}"]);
                    }
                    cBox.SetBinding(ComboBox.SelectedItemProperty, new Binding()
                    {
                        Converter = new EnumItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Condition,
                            LocalizationPrefix = $"{Type}_{prop.Name}_",
                            Type = propType
                        },
                        Path = new PropertyPath(propName),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    valueControl = cBox;
                }
                else if (propType == typeof(bool))
                {
                    valueControl = new CheckBox() { };
                    (valueControl as CheckBox).SetBinding(CheckBox.IsCheckedProperty, propName);
                }
                else
                {
                    App.Logger.LogWarning($"{propName} does not have required type '{propType.FullName}'");
                }
                valueControl.HorizontalAlignment = HorizontalAlignment.Right;
                valueControl.VerticalAlignment = VerticalAlignment.Center;
                borderContents.Children.Add(valueControl);
                valueControl.Tag = "variable::" + propName;
                Border b = new Border
                {
                    Child = borderContents
                };
                b.Margin = new Thickness(0, 5, 0, 5);
                if (prop.GetCustomAttribute<ConditionOptionalAttribute>() != null)
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
        public static string GetLocalizationKey(string typeName)
        {
            string s1 = typeName.Substring(9);
            StringBuilder sb = new StringBuilder();
            foreach (char c in s1)
            {
                if (char.IsUpper(c))
                {
                    sb.Append("_");
                }
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
