using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
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
    [XmlInclude(typeof(ConditionKillsTree))]
    [XmlInclude(typeof(ConditionCurrency))]
    public abstract class Condition : IHasUIText
    {
        [ConditionSkipField]
        public string Localization { get; set; }
        [XmlIgnore]
        public abstract Condition_Type Type { get; }
        [XmlIgnore]
        public abstract string UIText { get; }
        [ConditionNoValue]
        public bool Reset { get; set; }

        public IEnumerable<FrameworkElement> GetControls()
        {
            PropertyInfo[] props = GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (!prop.CanWrite || !prop.CanRead)
                {
                    continue;
                }

                string propName = prop.Name;
                Type propType = prop.PropertyType;
                // string localizedName = LocalizationManager.Current.Condition[$"{Type}_{propName}"];
                string localizedName;

                string key1 = $"{Type}_{propName}";
                string key2 = $"Shared_{propName}";

                if (LocalizationManager.Current.Condition.ContainsKey(key1))
                    localizedName = LocalizationManager.Current.Condition.Translate(key1);
                else if (LocalizationManager.Current.Condition.ContainsKey(key2))
                    localizedName = LocalizationManager.Current.Condition.Translate(key2);
                else
                    localizedName = key1;

                Grid borderContents = new Grid();
                Label l = new Label
                {
                    Content = localizedName
                };
                ConditionTooltipAttribute conditionTooltip = prop.GetCustomAttribute<ConditionTooltipAttribute>();
                if (conditionTooltip != null)
                {
                    l.ToolTip = conditionTooltip.Text;
                }
                borderContents.Children.Add(l);
                FrameworkElement valueControl = null;
                if (propType == typeof(ushort))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = ushort.MaxValue,
                        Minimum = ushort.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(uint))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = uint.MaxValue,
                        Minimum = uint.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(int))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = int.MaxValue,
                        Minimum = int.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(short))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = short.MaxValue,
                        Minimum = short.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(ushort))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = ushort.MaxValue,
                        Minimum = ushort.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(float))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = float.MaxValue,
                        Minimum = float.MinValue,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Float,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(byte))
                {
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = byte.MaxValue,
                        Minimum = byte.MinValue,
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
                    ComboBox cBox = new ComboBox();
                    Array values = Enum.GetValues(propType);
                    string prefix;
                    if (LocalizationManager.Current.Condition.ContainsKey($"{Type}_{prop.Name}"))
                        prefix = $"{Type}_{prop.Name}_";
                    else
                        prefix = $"Shared_{prop.Name}_";
                    if (propType == typeof(Logic_Type) && AppConfig.Instance.alternateLogicTranslation)
                        prefix += "Alternate_";
                    foreach (object eValue in values)
                    {
                        cBox.Items.Add(LocalizationManager.Current.Condition[$"{prefix}{eValue.ToString()}"]);
                    }
                    cBox.SetBinding(ComboBox.SelectedItemProperty, new Binding()
                    {
                        Converter = new EnumItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Condition,
                            LocalizationPrefix = prefix,
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
                    App.Logger.Log($"{propName} does not have required type '{propType.FullName}'");
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
                {
                    b.Opacity = 0.75;
                }

                yield return b;
            }
        }
        public static IEnumerable<Type> GetTypes()
        {
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
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

        public abstract bool Check(Simulation simulation);
        public abstract void Apply(Simulation simulation);
        public virtual string FormatCondition(Simulation simulation)
        {
            if (!string.IsNullOrEmpty(Localization))
            {
                return Localization;
            }

            return null;
        }
    }
}
