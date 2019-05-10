using BowieD.Unturned.NPCMaker.Localization;
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
        [RewardTooltip("Reward_Localization_Tooltip")]
        public string Localization;
        public abstract RewardType Type { get; }
        public abstract string DisplayName { get; }
        public IEnumerable<FrameworkElement> GetControls()
        {
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                string fieldName = field.Name;
                var fieldType = field.FieldType;
                string localizedName = LocUtil.LocalizeReward($"Reward_{fieldName}");
                Grid borderContents = new Grid();
                Label l = new Label();
                l.Content = localizedName;
                var rewardTooltip = field.GetCustomAttribute<RewardTooltipAttribute>();
                if (rewardTooltip != null)
                {
                    l.ToolTip = rewardTooltip.Text;
                }
                var rewardName = field.GetCustomAttribute<RewardNameAttribute>();
                if (rewardName != null)
                {
                    l.Content = rewardName.Text;
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
                            Content = LocUtil.LocalizeReward($"Reward_{field.Name}_Enum_{(eValue.ToString())}")
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
                if (t != null && t.IsSealed && t.IsSubclassOf(typeof(Reward)))
                {
                    yield return t;
                }
            }
        }
        public string GetFullFilePresentation(string prefix, int prefixIndex, int rewardIndex, bool skipLocalization = true)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            StringBuilder output = new StringBuilder();
            output.AppendLine($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{rewardIndex}_Type {Type.ToString()}");
            foreach (var field in this.GetType().GetFields())
            {
                var fieldName = field.Name;
                var skipFieldA = field.GetCustomAttribute<RewardSkipFieldAttribute>();
                if ((skipLocalization && fieldName == "Localization") || skipFieldA != null)
                    continue;
                var fieldValue = field.GetValue(this);
                var noValueA = field.GetCustomAttribute<RewardNoValueAttribute>();
                var optionalA = field.GetCustomAttribute<RewardOptionalAttribute>();
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
                output.AppendLine($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{rewardIndex}_{fieldName}{(fieldValue.ToString().Length > 0 ? " " + fieldValue.ToString() : "")}");
            }
            return output.ToString();
        }
    }

    #region ATTRIBUTES
    public class RewardNameAttribute : Attribute
    {
        public readonly string Text;
        public RewardNameAttribute(string key)
        {
            Text = LocUtil.LocalizeReward(key);
        }
    }
    public class RewardTooltipAttribute : Attribute
    {
        public readonly string Text;
        public RewardTooltipAttribute(string key)
        {
            Text = LocUtil.LocalizeReward(key);
        }
    }
    public class RewardOptionalAttribute : Attribute
    {
        public object requiredValue { get; private set; }
        public RewardOptionalAttribute(object requiredValue)
        {
            this.requiredValue = requiredValue;
        }
        public bool ConditionApplied(object currentValue)
        {
            if (requiredValue.Equals(currentValue))
                return true;
            return false;
        }
    }
    public class RewardNoValueAttribute : Attribute
    {
    }
    public class RewardSkipFieldAttribute : Attribute
    {
    }
    #endregion

    public sealed class RewardExperience : Reward
    {
        public override RewardType Type => RewardType.Experience;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardExperience")} x{Value}";
            }
        }

        public UInt32 Value;
    }
    public sealed class RewardQuest : Reward
    {
        public override RewardType Type => RewardType.Quest;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardQuest")} [{ID}]";
            }
        }
        public UInt16 ID;
    }
    public sealed class RewardItem : Reward
    {
        public override RewardType Type => RewardType.Item;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardItem")} {ID} x{Amount}";
            }
        }
        public UInt16 ID;
        public byte Amount;
        [RewardOptional(0)]
        public UInt16 Sight;
        [RewardOptional(0)]
        public UInt16 Tactical;
        [RewardOptional(0)]
        public UInt16 Grip;
        [RewardOptional(0)]
        public UInt16 Barrel;
        [RewardOptional(0)]
        public UInt16 Magazine;
        [RewardOptional(0)]
        public byte Ammo;
    }
    public sealed class RewardItemRandom : Reward
    {
        public override RewardType Type => RewardType.Item_Random;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardItemRandom")} [{ID}] x{Amount}";
            }
        }
        public UInt16 ID;
        public byte Amount;
    }
    public sealed class RewardReputation : Reward
    {
        public override RewardType Type => RewardType.Reputation;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardReputation")} x{Value}";
            }
        }
        public Int32 Value;
    }
    public sealed class RewardFlagBool : Reward
    {
        public override RewardType Type => RewardType.Flag_Bool;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardFlagBool")} [{ID}] -> {Value}";
            }
        }
        public UInt16 ID;
        public bool Value;
    }
    public sealed class RewardFlagShort : Reward
    {
        public override RewardType Type => RewardType.Flag_Short;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocUtil.LocalizeReward("Reward_Type_RewardFlagShort")} [{ID}] ");
                switch (Modification)
                {
                    case Modification_Type.Assign:
                        sb.Append("= ");
                        break;
                    case Modification_Type.Increment:
                        sb.Append("+ ");
                        break;
                    case Modification_Type.Decrement:
                        sb.Append("- ");
                        break;
                }
                sb.Append(Value);
                return sb.ToString();
            }
        }
        public UInt16 ID;
        public Int16 Value;
        public Modification_Type Modification;
    }
    public sealed class RewardFlagShortRandom : Reward
    {
        public override RewardType Type => RewardType.Flag_Short_Random;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocUtil.LocalizeReward("Reward_Type_RewardFlagShortRandom")} [{ID}] ");
                switch (Modification)
                {
                    case Modification_Type.Assign:
                        sb.Append("= ");
                        break;
                    case Modification_Type.Increment:
                        sb.Append("+ ");
                        break;
                    case Modification_Type.Decrement:
                        sb.Append("- ");
                        break;
                }
                sb.Append($"[{Min_Value};{Max_Value})");
                return sb.ToString();
            }
        }
        public UInt16 ID;
        public Int16 Min_Value;
        public Int16 Max_Value;
        public Modification_Type Modification;
    }
    public sealed class RewardTeleport : Reward
    {
        public override RewardType Type => RewardType.Teleport;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardTeleport")} [{Spawnpoint}]";
            }
        }

        public string Spawnpoint;
    }
    public sealed class RewardEvent : Reward
    {
        public override RewardType Type => RewardType.Event;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardEvent")} [{ID}]";
            }
        }

        public string ID;
    }
    public sealed class RewardFlagMath : Reward
    {
        public override RewardType Type => RewardType.Flag_Math;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"{LocUtil.LocalizeReward("Reward_Type_RewardFlagMath")} [{A_ID}] ");
                switch (Operation)
                {
                    case Operation_Type.Addition:
                        sb.Append("+ ");
                        break;
                    case Operation_Type.Assign:
                        sb.Append("= ");
                        break;
                    case Operation_Type.Division:
                        sb.Append("/ ");
                        break;
                    case Operation_Type.Multiplication:
                        sb.Append("* ");
                        break;
                    case Operation_Type.Subtraction:
                        sb.Append("- ");
                        break;
                }
                sb.Append($"[{B_ID}]");
                return sb.ToString();
            }
        }
        public UInt16 A_ID;
        public UInt16 B_ID;
        public Operation_Type Operation;
    }
    public sealed class RewardVehicle : Reward
    {
        public override RewardType Type => RewardType.Vehicle;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardVehicle")} [{ID}] -> [{Spawnpoint}]";
            }
        }
        public UInt16 ID;
        public string Spawnpoint;
    }
    public sealed class RewardAchievement : Reward
    {
        public override RewardType Type => RewardType.Achievement;
        public override string DisplayName
        {
            get
            {
                return $"{LocUtil.LocalizeReward("Reward_Type_RewardAchievement")} [{ID}]";
            }
        }
        public string ID;
    }
}
