using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
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
        [Context(ContextHelper.EContextOption.Group_TextEdit | ContextHelper.EContextOption.Group_Rich)]
        [ApplicableToOpenCloseBoomerangs]
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

            if (version == -1)
            {
                switch (typeAt.Value)
                {
                    case "Experience_Cond":
                        return new ConditionExperience();
                    case "Flag_Bool_Cond":
                        return new ConditionFlagBool();
                    case "Flag_Short_Cond":
                        return new ConditionFlagShort();
                    case "Item_Cond":
                        return new ConditionItem();
                    case "Kills_Animal_Cond":
                        return new ConditionKillsAnimal();
                    case "Kills_Horde_Cond":
                        return new ConditionKillsHorde();
                    case "Kills_Object_Cond":
                        return new ConditionKillsObject();
                    case "Kills_Players_Cond":
                        return new ConditionKillsPlayer();
                    case "Kills_Zombie_Cond":
                        return new ConditionKillsZombie();
                    case "Player_Life_Food_Cond":
                        return new ConditionPlayerLifeFood();
                    case "Player_Life_Health_Cond":
                        return new ConditionPlayerLifeHealth();
                    case "Player_Life_Virus_Cond":
                        return new ConditionPlayerLifeVirus();
                    case "Player_Life_Water_Cond":
                        return new ConditionPlayerLifeWater();
                    case "Quest_Cond":
                        return new ConditionQuest();
                    case "Reputation_Cond":
                        return new ConditionReputation();
                    case "Skillset_Cond":
                        return new ConditionSkillset();
                    case "Time_Of_Day_Cond":
                        return new ConditionTimeOfDay();
                    default:
                        throw new Exception("Unknown type");
                }
            }
            else
            {
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
            }
        });
        public Func<XmlNode, int, Condition> CreateFromNodeFunction => _createFunc;

        public virtual string TypeName => GetType().Name;

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
                TextBoxOptionsAttribute textBoxOptionsAttribute = prop.GetCustomAttribute<TextBoxOptionsAttribute>();
                RangeAttribute rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
                AssetPickerAttribute assetPickerAttribute = prop.GetCustomAttribute<AssetPickerAttribute>();
                ApplicableToOpenCloseBoomerangsAttribute openCloseBoomerangsAttribute = prop.GetCustomAttribute<ApplicableToOpenCloseBoomerangsAttribute>();
                ContextAttribute contextAttribute = prop.GetCustomAttribute<ContextAttribute>();
                CanUseAlternateBoolAttribute canUseAlternateBoolAttribute = prop.GetCustomAttribute<CanUseAlternateBoolAttribute>();
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
                            (valueControl as MahApps.Metro.Controls.NumericUpDown).Value = asset.ID;
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
                            (valueControl as Controls.OptionalUInt16ValueControl).upDown.Value = asset.ID;
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
                    if (textBoxOptionsAttribute is null)
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
                                (valueControl as TextBox).Text = asset.GUID.ToString("N");
                            }, assetPickerAttribute.Key, assetPickerAttribute.Icon));
                            valueControl.ContextMenu = vcMenu;
                        }

                        if (openCloseBoomerangsAttribute != null)
                        {
                            IDELikeTool.RegisterOpenCloseBoomerangs(valueControl as TextBox);
                        }
                    }
                    else
                    {
                        valueControl = new ComboBox()
                        {
                            IsEditable = true,
                            ItemsSource = textBoxOptionsAttribute.Options,
                            MaxWidth = 100,
                        };
                        (valueControl as ComboBox).SetBinding(ComboBox.TextProperty, propName);

                        if (assetPickerAttribute != null)
                        {
                            var vcMenu = new ContextMenu();
                            vcMenu.Items.Add(ContextHelper.CreateSelectAssetButton(assetPickerAttribute.AssetType, (asset) =>
                            {
                                (valueControl as ComboBox).Text = asset.GUID.ToString("N");
                            }, assetPickerAttribute.Key, assetPickerAttribute.Icon));
                            valueControl.ContextMenu = vcMenu;
                        }
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
                    if (AppConfig.Instance.alternateBoolValue && canUseAlternateBoolAttribute != null)
                    {
                        ComboBox cbox = new ComboBox();

                        BoolItemsSource bis = new BoolItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Condition,
                            LocalizationPrefix = $"{Type}_{prop.Name}_",
                        };

                        cbox.ItemsSource = bis;
                        cbox.SetBinding(ComboBox.SelectedItemProperty, new Binding(propName)
                        {
                            Converter = bis,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        });

                        valueControl = cbox;
                    }
                    else
                    {
                        valueControl = new CheckBox() { };
                        (valueControl as CheckBox).SetBinding(CheckBox.IsCheckedProperty, propName);
                    }
                }
                else if (propType == typeof(TimeSpan))
                {
                    valueControl = new ClockControl();

                    (valueControl as ClockControl).SetBinding(ClockControl.DisplayTimeProperty, new Binding(propName) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                }
                else if (propType == typeof(GUIDIDBridge))
                {
                    valueControl = new GUIDIDControl();
                    (valueControl as GUIDIDControl).SetBinding(GUIDIDControl.ValueProperty, propName);

                    if (assetPickerAttribute != null)
                    {
                        var vcMenu = new ContextMenu();
                        vcMenu.Items.Add(ContextHelper.CreateSelectAssetButton(assetPickerAttribute.AssetType, (asset) =>
                        {
                            if (AppConfig.Instance.preferLegacyIDsOverGUIDs)
                                (valueControl as GUIDIDControl).Value = new GUIDIDBridge(asset.ID);
                            else
                                (valueControl as GUIDIDControl).Value = new GUIDIDBridge(asset.GUID);
                        }, assetPickerAttribute.Key, assetPickerAttribute.Icon));

                        (valueControl as GUIDIDControl).ContextMenu = vcMenu;
                    }
                }
                else
                {
                    App.Logger.Log($"{propName} does not have required type '{propType.FullName}'");
                }
                valueControl.HorizontalAlignment = HorizontalAlignment.Right;
                valueControl.VerticalAlignment = VerticalAlignment.Center;
                if (contextAttribute != null)
                    valueControl.ContextMenu = ContextHelper.CreateContextMenu(contextAttribute.Options);
                borderContents.Children.Add(valueControl);

                borderContents.ColumnDefinitions.Add(new ColumnDefinition());
                borderContents.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(valueControl, 1);

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
                    var skillLock = t.GetCustomAttribute<Configuration.SkillLockAttribute>();

                    if (skillLock is null || AppConfig.Instance.skillLevel >= skillLock.Level)
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
            Localization = node["Localization"].ToText();
            Reset = node["Reset"].ToBoolean();
        }

        public virtual void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("Localization", node).WriteString(Localization);
            document.CreateNodeC("Reset", node).WriteBoolean(Reset);
        }
    }
}
