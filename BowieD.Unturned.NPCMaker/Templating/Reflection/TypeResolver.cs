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
                            case "compareflags":
                            case "compare_flags":
                                return typeof(ConditionCompareFlags);
                            case "currency":
                                return typeof(ConditionCurrency);
                            case "experience":
                                return typeof(ConditionExperience);
                            case "flagbool":
                            case "flagboolean":
                            case "flag_bool":
                            case "flag_boolean":
                                return typeof(ConditionFlagBool);
                            case "flagshort":
                            case "flagint16":
                            case "flag_short":
                            case "flag_int16":
                                return typeof(ConditionFlagShort);
                            case "holiday": // also check out Boulevard of Broken Dreams
                                return typeof(ConditionHoliday);
                            case "item":
                                return typeof(ConditionItem);
                            case "kills_animal":
                            case "killsanimal": // peta won't be happy
                                return typeof(ConditionKillsAnimal);
                            case "kills_horde":
                            case "killshorde":
                                return typeof(ConditionKillsHorde);
                            case "kills_object":
                            case "killsobject":
                                return typeof(ConditionKillsObject);
                            case "kills_player":
                            case "killsplayer":
                                return typeof(ConditionKillsPlayer);
                            case "kills_tree":
                            case "killstree": // mr beast won't be happy
                                return typeof(ConditionKillsTree);
                            case "player_life_food":
                            case "playerlifefood":
                                return typeof(ConditionPlayerLifeFood);
                            case "player_life_health":
                            case "playerlifehealth":
                                return typeof(ConditionPlayerLifeHealth);
                            case "player_life_virus":
                            case "playerlifevirus":
                                return typeof(ConditionPlayerLifeVirus);
                            case "player_life_water":
                            case "playerlifewater":
                                return typeof(ConditionPlayerLifeWater);
                            case "quest":
                                return typeof(ConditionQuest);
                            case "reputation":
                                return typeof(ConditionReputation);
                            case "skillset":
                                return typeof(ConditionSkillset);
                            case "time_of_day":
                            case "timeofday":
                                return typeof(ConditionTimeOfDay);
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
