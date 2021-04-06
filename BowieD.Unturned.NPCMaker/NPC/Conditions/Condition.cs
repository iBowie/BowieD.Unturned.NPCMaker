﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using BowieD.Unturned.NPCMaker.XAML;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [Serializable]
    public class Condition : IHasUIText, IAXDataDerived<Condition>
    {
        [SkipField]
        public string Localization { get; set; }
        [XmlIgnore]
        public virtual Condition_Type Type => throw new NotImplementedException();
        [XmlIgnore]
        public virtual string UIText => throw new NotImplementedException();
        [NoValue]
        public bool Reset { get; set; }

        private static Func<XmlNode, int, Condition> _createFunc = new Func<XmlNode, int, Condition>((node, version) =>
        {
            var typeAt = node.Attributes["xsi:type"];

            switch (typeAt.Value)
            {
                case "ConditionCompareFlags":
                    return new ConditionCompareFlags();
                case "ConditionCurrency":
                    return new ConditionCurrency();
                case "ConditionExperience":
                    return new ConditionExperience();
                case "ConditionFlagBool":
                    return new ConditionFlagBool();
                case "ConditionFlagShort":
                    return new ConditionFlagShort();
                case "ConditionHoliday":
                    return new ConditionHoliday();
                case "ConditionItem":
                    return new ConditionItem();
                case "ConditionKillsAnimal":
                    return new ConditionKillsAnimal();
                case "ConditionKillsHorde":
                    return new ConditionKillsHorde();
                case "ConditionKillsObject":
                    return new ConditionKillsObject();
                case "ConditionKillsPlayer":
                    return new ConditionKillsPlayer();
                case "ConditionKillsTree":
                    return new ConditionKillsTree();
                case "ConditionKillsZombie":
                    return new ConditionKillsZombie();
                case "ConditionPlayerLifeFood":
                    return new ConditionPlayerLifeFood();
                case "ConditionPlayerLifeHealth":
                    return new ConditionPlayerLifeHealth();
                case "ConditionPlayerLifeVirus":
                    return new ConditionPlayerLifeVirus();
                case "ConditionPlayerLifeWater":
                    return new ConditionPlayerLifeWater();
                case "ConditionQuest":
                    return new ConditionQuest();
                case "ConditionReputation":
                    return new ConditionReputation();
                case "ConditionSkillset":
                    return new ConditionSkillset();
                case "ConditionTimeOfDay":
                    return new ConditionTimeOfDay();
                case "ConditionWeatherBlendAlpha":
                    return new ConditionWeatherBlendAlpha();
                case "ConditionWeatherStatus":
                    return new ConditionWeatherStatus();
                default:
                    throw new Exception("Unknown type");
            }
        });
        public Func<XmlNode, int, Condition> CreateFromNodeFunction => _createFunc;

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
                TooltipAttribute conditionTooltip = prop.GetCustomAttribute<TooltipAttribute>();
                if (conditionTooltip != null)
                {
                    l.ToolTip = conditionTooltip.Text;
                }
                else if (LocalizationManager.Current.Condition.ContainsKey($"{key1}_Tooltip"))
                {
                    l.ToolTip = LocalizationManager.Current.Condition[$"{key1}_Tooltip"];
                }
                else if (LocalizationManager.Current.Condition.ContainsKey($"{key2}_Tooltip"))
                {
                    l.ToolTip = LocalizationManager.Current.Condition[$"{key2}_Tooltip"];
                }

                borderContents.Children.Add(l);
                RangeAttribute rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
                AssetPickerAttribute assetPickerAttribute = prop.GetCustomAttribute<AssetPickerAttribute>();
                FrameworkElement valueControl = null;
                if (propType == typeof(ushort))
                {
                    ushort newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is ushort rMax && rangeAttribute.Minimum is ushort rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = ushort.MaxValue;
                        newMin = ushort.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);

                    if (assetPickerAttribute != null)
                    {
                        var vcMenu = new ContextMenu();
                        vcMenu.Items.Add(ContextHelper.CreateSelectAssetButton(assetPickerAttribute.AssetType, (asset) =>
                        {
                            (valueControl as MahApps.Metro.Controls.NumericUpDown).Value = asset.id;
                        }, assetPickerAttribute.Key, assetPickerAttribute.Icon));
                        (valueControl as MahApps.Metro.Controls.NumericUpDown).ContextMenu = vcMenu;
                    }
                }
                else if (propType == typeof(byte?))
                {
                    byte newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is byte rMax && rangeAttribute.Minimum is byte rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = byte.MaxValue;
                        newMin = byte.MinValue;
                    }
                    valueControl = new Controls.OptionalByteValueControl();
                    (valueControl as Controls.OptionalByteValueControl).upDown.Maximum = newMax;
                    (valueControl as Controls.OptionalByteValueControl).upDown.Minimum = newMin;
                    (valueControl as Controls.OptionalByteValueControl).upDown.SetBinding(Xceed.Wpf.Toolkit.ByteUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(int?))
                {
                    int newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is int rMax && rangeAttribute.Minimum is int rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = int.MaxValue;
                        newMin = int.MinValue;
                    }
                    valueControl = new Controls.OptionalInt32ValueControl();
                    (valueControl as Controls.OptionalInt32ValueControl).upDown.Maximum = newMax;
                    (valueControl as Controls.OptionalInt32ValueControl).upDown.Minimum = newMin;
                    (valueControl as Controls.OptionalInt32ValueControl).upDown.SetBinding(Xceed.Wpf.Toolkit.IntegerUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(ushort?))
                {
                    ushort newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is ushort rMax && rangeAttribute.Minimum is ushort rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = ushort.MaxValue;
                        newMin = ushort.MinValue;
                    }
                    valueControl = new Controls.OptionalUInt16ValueControl();
                    (valueControl as Controls.OptionalUInt16ValueControl).upDown.Maximum = newMax;
                    (valueControl as Controls.OptionalUInt16ValueControl).upDown.Minimum = newMin;
                    (valueControl as Controls.OptionalUInt16ValueControl).upDown.SetBinding(Xceed.Wpf.Toolkit.UShortUpDown.ValueProperty, propName);

                    if (assetPickerAttribute != null)
                    {
                        var vcMenu = new ContextMenu();
                        vcMenu.Items.Add(ContextHelper.CreateSelectAssetButton(assetPickerAttribute.AssetType, (asset) =>
                        {
                            (valueControl as Controls.OptionalUInt16ValueControl).checkbox.IsChecked = true;
                            (valueControl as Controls.OptionalUInt16ValueControl).upDown.Value = asset.id;
                        }, assetPickerAttribute.Key, assetPickerAttribute.Icon));
                        valueControl.ContextMenu = vcMenu;
                        (valueControl as Controls.OptionalUInt16ValueControl).upDown.ContextMenu = vcMenu;
                        (valueControl as Controls.OptionalUInt16ValueControl).checkbox.ContextMenu = vcMenu;
                    }
                }
                else if (propType == typeof(uint))
                {
                    uint newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is uint rMax && rangeAttribute.Minimum is uint rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = uint.MaxValue;
                        newMin = uint.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(int))
                {
                    int newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is int rMax && rangeAttribute.Minimum is int rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = int.MaxValue;
                        newMin = int.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(short))
                {
                    short newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is short rMax && rangeAttribute.Minimum is short rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = short.MaxValue;
                        newMin = short.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Integer,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(float))
                {
                    float newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is float rMax && rangeAttribute.Minimum is float rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = float.MaxValue;
                        newMin = float.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
                        ParsingNumberStyle = System.Globalization.NumberStyles.Float,
                        HideUpDownButtons = true
                    };
                    (valueControl as MahApps.Metro.Controls.NumericUpDown).SetBinding(MahApps.Metro.Controls.NumericUpDown.ValueProperty, propName);
                }
                else if (propType == typeof(byte))
                {
                    byte newMax, newMin;
                    if (rangeAttribute != null && rangeAttribute.Maximum is byte rMax && rangeAttribute.Minimum is byte rMin)
                    {
                        newMax = rMax;
                        newMin = rMin;
                    }
                    else
                    {
                        newMax = byte.MaxValue;
                        newMin = byte.MinValue;
                    }
                    valueControl = new MahApps.Metro.Controls.NumericUpDown()
                    {
                        Maximum = newMax,
                        Minimum = newMin,
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

                    if (assetPickerAttribute != null)
                    {
                        var vcMenu = new ContextMenu();
                        vcMenu.Items.Add(ContextHelper.CreateSelectAssetButton(assetPickerAttribute.AssetType, (asset) =>
                        {
                            (valueControl as TextBox).Text = asset.guid.ToString("N");
                        }, assetPickerAttribute.Key, assetPickerAttribute.Icon));
                        (valueControl as TextBox).ContextMenu = vcMenu;
                    }
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

        public virtual void PostLoad(Universal_ConditionEditor editor) { }

        public virtual bool Check(Simulation simulation) => throw new Exception();
        public virtual void Apply(Simulation simulation) => throw new Exception();
        public virtual string FormatCondition(Simulation simulation)
        {
            if (!string.IsNullOrEmpty(Localization))
            {
                return Localization;
            }

            return null;
        }

        public virtual void Load(XmlNode node, int version)
        {
            Localization = node["Localization"].InnerText;
            Reset = node["Reset"].ToBoolean();
        }

        public virtual void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("Localization", node).WriteString(Localization);
            document.CreateNodeC("Reset", node).WriteBoolean(Reset);
        }
    }
}
