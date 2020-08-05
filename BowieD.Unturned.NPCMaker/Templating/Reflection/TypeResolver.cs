using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
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
                case "byte":
                    return typeof(byte);
                case "byte?":
                    return typeof(byte?);
                case "ushort":
                case "uint16":
                    return typeof(ushort);
                case "ushort?":
                case "uint16?":
                    return typeof(ushort?);
                case "uint":
                case "uint32":
                    return typeof(uint);
                case "uint?":
                case "uint32?":
                    return typeof(uint?);
                case "ulong":
                case "uint64":
                    return typeof(ulong);
                case "ulong?":
                case "uint64?":
                    return typeof(ulong?);
                case "short":
                case "int16":
                    return typeof(short);
                case "short?":
                case "int16?":
                    return typeof(short?);
                case "int":
                case "int32":
                    return typeof(int);
                case "int?":
                case "int32?":
                    return typeof(int?);
                case "long":
                case "int64":
                    return typeof(long);
                case "long?":
                case "int64?":
                    return typeof(long?);
                case "string":
                    return typeof(string);
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
                case "npcresponse":
                case "response":
                case "reply":
                    return typeof(NPCResponse);
                case "dialogue":
                case "npcdialogue":
                    return typeof(NPCDialogue);
                case "character":
                case "npccharacter":
                    return typeof(NPCCharacter);
                case "clothing":
                case "npcclothing":
                    return typeof(NPCClothing);
                case "message":
                case "npcmessage":
                    return typeof(NPCMessage);
                case "project":
                case "npcproject":
                    return typeof(NPCProject);
                case "vendor":
                case "npcvendor":
                    return typeof(NPCVendor);
                default:
                    return null;
            }
        }
    }
}
