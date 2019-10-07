using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Rewards.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC.Rewards
{
    [XmlInclude(typeof(RewardExperience))]
    [XmlInclude(typeof(RewardReputation))]
    [XmlInclude(typeof(RewardFlagBool))]
    [XmlInclude(typeof(RewardFlagShort))]
    [XmlInclude(typeof(RewardFlagShortRandom))]
    [XmlInclude(typeof(RewardQuest))]
    [XmlInclude(typeof(RewardItem))]
    [XmlInclude(typeof(RewardItemRandom))]
    [XmlInclude(typeof(RewardAchievement))]
    [XmlInclude(typeof(RewardVehicle))]
    [XmlInclude(typeof(RewardTeleport))]
    [XmlInclude(typeof(RewardEvent))]
    [XmlInclude(typeof(RewardFlagMath))]
    public abstract class Reward : IHasDisplayName
    {
        [RewardSkipField]
        public string Localization;
        public abstract RewardType Type { get; }
        public abstract string GameName { get; }
        public IEnumerable<FrameworkElement> GetControls()
        {
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                string fieldName = field.Name;
                var fieldType = field.FieldType;
                string localizedName = LocalizationManager.Current.Reward[$"{Type}_{fieldName}"];
                Grid borderContents = new Grid();
                Label l = new Label
                {
                    Content = localizedName
                };
                var rewardTooltip = field.GetCustomAttribute<RewardTooltipAttribute>();
                if (rewardTooltip != null)
                {
                    l.ToolTip = rewardTooltip.Text;
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
                            Content = LocalizationManager.Current.Reward[$"{Type}_{field.Name}_{eValue.ToString()}"]
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
                if (field.GetCustomAttribute<RewardOptionalAttribute>() != null)
                    b.Opacity = 0.75;
                yield return b;
            }
        }
        public static IEnumerable<Type> GetTypes()
        {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t != null && t.IsSealed && t.IsSubclassOf(typeof(Reward)))
                {
                    yield return t;
                }
            }
        }
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
    }
}
