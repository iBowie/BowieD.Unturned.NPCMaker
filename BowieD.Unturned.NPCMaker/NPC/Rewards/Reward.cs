using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using BowieD.Unturned.NPCMaker.XAML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [System.Serializable]
    public class Reward : IHasUIText, IAXDataDerived<Reward>
    {
        [SkipField]
        [Context(ContextHelper.EContextOption.Group_TextEdit | ContextHelper.EContextOption.Group_Rich)]
        [ApplicableToOpenCloseBoomerangs]
        public string Localization { get; set; }
        public virtual RewardType Type => throw new NotImplementedException();
        public virtual string UIText => throw new NotImplementedException();

        private static Func<XmlNode, int, Reward> _createFunc = new Func<XmlNode, int, Reward>((node, version) =>
        {
            var typeAt = node.Attributes["xsi:type"];

            if (version == -1)
            {
                switch (typeAt.Value)
                {
                    case "Experience":
                        return new RewardExperience();
                    case "Flag_Bool":
                        return new RewardFlagBool();
                    case "Flag_Math":
                        return new RewardFlagMath();
                    case "Flag_Short":
                        return new RewardFlagShort();
                    case "Flag_Short_Random":
                        return new RewardFlagShortRandom();
                    case "Item":
                        return new RewardItem();
                    case "Item_Random":
                        return new RewardItemRandom();
                    case "Quest":
                        return new RewardQuest();
                    case "Reputation":
                        return new RewardReputation();
                    case "Teleport":
                        return new RewardTeleport();
                    case "Vehicle":
                        return new RewardVehicle();
                    default:
                        throw new Exception("Unknown type");
                }
            }
            else
            {
                switch (typeAt.Value)
                {
                    case "RewardAchievement":
                        return new RewardAchievement();
                    case "RewardCurrency":
                        return new RewardCurrency();
                    case "RewardEvent":
                        return new RewardEvent();
                    case "RewardExperience":
                        return new RewardExperience();
                    case "RewardFlagBool":
                        return new RewardFlagBool();
                    case "RewardFlagMath":
                        return new RewardFlagMath();
                    case "RewardFlagShort":
                        return new RewardFlagShort();
                    case "RewardFlagShortRandom":
                        return new RewardFlagShortRandom();
                    case "RewardHint":
                        return new RewardHint();
                    case "RewardItem":
                        return new RewardItem();
                    case "RewardItemRandom":
                        return new RewardItemRandom();
                    case "RewardQuest":
                        return new RewardQuest();
                    case "RewardReputation":
                        return new RewardReputation();
                    case "RewardTeleport":
                        return new RewardTeleport();
                    case "RewardVehicle":
                        return new RewardVehicle();
                    case "RewardPlayerSpawnpoint" when version >= 12:
                        return new RewardPlayerSpawnpoint();
                    default:
                        throw new Exception("Unknown type");
                }
            }
        });

        public Func<XmlNode, int, Reward> CreateFromNodeFunction => _createFunc;

        public virtual string TypeName => GetType().Name;

        public IEnumerable<FrameworkElement> GetControls()
        {
            IEnumerable<PropertyInfo> props = GetType().GetProperties()
                .Where(pInfo => pInfo.CanRead && pInfo.CanWrite)
                .OrderByDescending(pInfo =>
                {
                    var orderAttrib = pInfo.GetCustomAttribute<PriorityAttribute>();

                    int priority = orderAttrib?.Priority ?? 0;

                    return priority;
                });
            
            foreach (PropertyInfo prop in props)
            {
                string propName = prop.Name;
                Type propType = prop.PropertyType;

                string localizedName;

                string key1 = $"{Type}_{propName}";
                string key2 = $"Shared_{propName}";

                if (LocalizationManager.Current.Reward.ContainsKey(key1))
                    localizedName = LocalizationManager.Current.Reward.Translate(key1);
                else if (LocalizationManager.Current.Reward.ContainsKey(key2))
                    localizedName = LocalizationManager.Current.Reward.Translate(key2);
                else
                    localizedName = key1;

                Grid borderContents = new Grid();
                Label l = new Label
                {
                    Content = localizedName
                };
                TooltipAttribute rewardTooltip = prop.GetCustomAttribute<TooltipAttribute>();
                if (rewardTooltip != null)
                {
                    l.ToolTip = rewardTooltip.Text;
                }
                else if (LocalizationManager.Current.Reward.ContainsKey($"{key1}_Tooltip"))
                {
                    l.ToolTip = LocalizationManager.Current.Reward[$"{key1}_Tooltip"];
                }
                else if (LocalizationManager.Current.Reward.ContainsKey($"{key2}_Tooltip"))
                {
                    l.ToolTip = LocalizationManager.Current.Reward[$"{key2}_Tooltip"];
                }

                TextBoxOptionsAttribute textBoxOptionsAttribute = prop.GetCustomAttribute<TextBoxOptionsAttribute>();
                RangeAttribute rangeAttribute = prop.GetCustomAttribute<RangeAttribute>();
                AssetPickerAttribute assetPickerAttribute = prop.GetCustomAttribute<AssetPickerAttribute>();
                ApplicableToOpenCloseBoomerangsAttribute openCloseBoomerangsAttribute = prop.GetCustomAttribute<ApplicableToOpenCloseBoomerangsAttribute>();
                ContextAttribute contextAttribute = prop.GetCustomAttribute<ContextAttribute>();
                CanUseAlternateBoolAttribute canUseAlternateBoolAttribute = prop.GetCustomAttribute<CanUseAlternateBoolAttribute>();
                borderContents.Children.Add(l);
                FrameworkElement valueControl = null;
                if (propType == typeof(uint))
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
                    foreach (object eValue in values)
                    {
                        cBox.Items.Add(LocalizationManager.Current.Reward[$"{Type}_{prop.Name}_{eValue.ToString()}"]);
                    }
                    cBox.SetBinding(ComboBox.SelectedItemProperty, new Binding()
                    {
                        Converter = new EnumItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Reward,
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
                    if (AppConfig.Instance.alternateBoolValue && canUseAlternateBoolAttribute != null)
                    {
                        ComboBox cbox = new ComboBox();

                        BoolItemsSource bis = new BoolItemsSource()
                        {
                            Dictionary = LocalizationManager.Current.Reward,
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
                if (t != null && t.IsSealed && t.IsSubclassOf(typeof(Reward)))
                {
                    var skillLock = t.GetCustomAttribute<Configuration.SkillLockAttribute>();

                    if (skillLock is null || AppConfig.Instance.skillLevel >= skillLock.Level)
                        yield return t;
                }
            }
        }
        public virtual void PostLoad(Universal_RewardEditor editor) { }

        public static string GetLocalizationKey(string typeName)
        {
            string s1 = typeName.Substring(6);
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

        public virtual void Give(Simulation simulation) => throw new NotImplementedException();
        public virtual string FormatReward(Simulation simulation)
        {
            return null;
        }

        public virtual void Load(XmlNode node, int version)
        {
            Localization = node["Localization"].ToText();
        }

        public virtual void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("Localization", node).WriteString(Localization);
        }
    }
}
