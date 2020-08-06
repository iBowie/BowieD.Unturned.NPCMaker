using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using BowieD.Unturned.NPCMaker.Templating.Modify.Converters;
using System;

namespace BowieD.Unturned.NPCMaker.Templating.Reflection
{
    public static class TypeResolver
    {
        /// <param name="parameter">Mostly used to specify input field name, or condition type</param>
        public static Type Resolve(string name, object parameter, Template template)
        {
            switch (name.ToLowerInvariant())
            {
                case "condition" when !Object.ReferenceEquals(parameter, null):
                    {
                        string cType = parameter.ToString().ToLowerInvariant();

                        switch (cType)
                        {
                            case "item":
                                return typeof(ConditionItem);
                            default:
                                return null;
                        }
                    }
                case "reward" when !Object.ReferenceEquals(parameter, null):
                    {
                        string rType = parameter.ToString().ToLowerInvariant();

                        switch (rType)
                        {
                            case "achievement":
                                return typeof(RewardAchievement);
                            case "currency":
                                return typeof(RewardCurrency);
                            case "event":
                                return typeof(RewardEvent);
                            case "experience":
                                return typeof(RewardExperience);
                            case "flagbool":
                            case "flag_bool":
                            case "flagboolean":
                            case "flag_boolean":
                                return typeof(RewardFlagBool);
                            case "flagmath":
                            case "flag_math":
                                return typeof(RewardFlagMath);
                            case "flagshort":
                            case "flag_short":
                                return typeof(RewardFlagShort);
                            case "flagshortrandom":
                            case "flag_short_random":
                                return typeof(RewardFlagShortRandom);
                            case "hint":
                                return typeof(RewardHint);
                            case "item":
                                return typeof(RewardItem);
                            case "item_random":
                            case "itemrandom":
                                return typeof(RewardItemRandom);
                            case "quest":
                                return typeof(RewardQuest);
                            case "reputation":
                                return typeof(RewardReputation);
                            case "teleport":
                                return typeof(RewardTeleport);
                            case "vehicle":
                                return typeof(RewardVehicle);
                            default:
                                return null;
                        }
                    }
                case "input" when !Object.ReferenceEquals(parameter, null):
                    {
                        if (template.Inputs.TryGetValue(parameter.ToString(), out var inputField))
                        {
                            return Resolve(inputField.Type, null, template);
                        }
                        else
                            goto default;
                    }
                default:
                    {
                        if (ModifyValueConverter.Types.TryGetValue(name, out var typeInfo))
                            return typeInfo.InnerType;
                    }
                    return null;
            }
        }
    }
}
