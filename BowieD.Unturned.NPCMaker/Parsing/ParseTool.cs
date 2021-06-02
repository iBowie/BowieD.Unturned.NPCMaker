using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public sealed class ParseTool
    {
        private readonly DataReader local;
        private readonly DataReader asset;
        public ParseTool(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;
            asset = new DataReader(File.ReadAllText(fileName));
            if (File.Exists(dir + "English.dat"))
            {
                local = new DataReader(File.ReadAllText(dir + "English.dat"));
            }
            else
            {
                foreach (object k in Enum.GetValues(typeof(ELanguage)))
                {
                    App.Logger.Log($"[ParseTool] - English.dat not found. Checking all languages...");
                    if (File.Exists(dir + $"{k}.dat"))
                    {
                        local = new DataReader(File.ReadAllText(dir + $"{k}.dat"));
                        break;
                    }
                    else
                    {
                        App.Logger.Log($"[ParseTool] - {k}.dat not found. Checking next...");
                    }
                }
            }
        }
        public ParseTool(DataReader asset, DataReader local)
        {
            this.asset = asset;
            this.local = local;
        }
        public NPCCharacter ParseCharacter()
        {
            return new NPCCharacter
            {
                ID = asset.ReadUInt16("ID"),
                beard = asset.ReadByte("Beard"),
                face = asset.ReadByte("Face"),
                haircut = asset.ReadByte("Hair"),
                hairColor = asset.ReadColor("Color_Hair"),
                skinColor = asset.ReadColor("Color_Skin"),
                equipPrimary = asset.ReadUInt16("Primary"),
                equipSecondary = asset.ReadUInt16("Secondary"),
                equipTertiary = asset.ReadUInt16("Tertiary"),
                startDialogueId = asset.ReadUInt16("Dialogue"),
                DisplayName = local?.ReadString("Character"),
                EditorName = local?.ReadString("Name"),
                GUID = asset.ReadGUID("GUID", Guid.NewGuid()).ToString("N"),
                leftHanded = asset.Has("Backward"),
                clothing = ParseClothing(Clothing_Type.Default),
                christmasClothing = ParseClothing(Clothing_Type.Christmas),
                halloweenClothing = ParseClothing(Clothing_Type.Halloween),
                pose = asset.ReadEnum("Pose", NPC_Pose.Stand),
                poseHeadOffset = asset.ReadSingle("Pose_Head_Offset"),
                poseLean = asset.ReadSingle("Pose_Lean"),
                posePitch = asset.ReadSingle("Pose_Pitch", 90f),
                equipped = asset.ReadEnum("Equipped", Equip_Type.None),
                visibilityConditions = ParseConditions("").ToLimitedList(byte.MaxValue)
            };
        }
        public NPCDialogue ParseDialogue()
        {
            NPCDialogue d = new NPCDialogue()
            {
                GUID = asset.ReadGUID("GUID", Guid.NewGuid()).ToString("N"),
                ID = asset.ReadUInt16("ID")
            };
            byte msgCount = asset.ReadByte("Messages");
            d.Messages = new LimitedList<NPCMessage>(byte.MaxValue);
            Dictionary<ushort, byte[]> m_visible = new Dictionary<ushort, byte[]>();
            for (byte mId = 0; mId < msgCount; mId++)
            {
                string[] pages = new string[asset.ReadByte($"Message_{mId}_Pages")];
                for (byte pId = 0; pId < pages.Length; pId++)
                {
                    string page = local?.ReadString($"Message_{mId}_Page_{pId}");
                    if (page == null)
                    {
                        App.Logger.Log($"Page {pId} in message {mId} not found.");
                    }
                    pages[pId] = page ?? string.Empty;
                }
                byte[] array2 = new byte[asset.ReadByte($"Message_{mId}_Responses")];
                for (byte rId = 0; rId < array2.Length; rId++)
                {
                    array2[rId] = asset.ReadByte($"Message_{mId}_Response_{rId}");
                }
                m_visible.Add(mId, array2);

                d.Messages.Add(new NPCMessage()
                {
                    conditions = ParseConditions($"Message_{mId}_").ToLimitedList(byte.MaxValue),
                    pages = pages.ToLimitedList(byte.MaxValue),
                    prev = asset.ReadUInt16($"Message_{mId}_Prev"),
                    rewards = ParseRewards($"Message_{mId}_").ToLimitedList(byte.MaxValue)
                });
            }
            byte rspCount = asset.ReadByte("Responses");
            d.Responses = new LimitedList<NPCResponse>(byte.MaxValue);
            for (byte rId = 0; rId < rspCount; rId++)
            {
                d.Responses.Add(new NPCResponse());
                byte b = asset.ReadByte($"Response_{rId}_Messages");
                d.Responses[rId].visibleIn = new int[d.Messages.Count];
                for (byte i = 0; i < d.Responses[rId].visibleIn.Length; i++)
                {
                    if (m_visible.TryGetValue(i, out var pages))
                    {
                        if (pages.Contains(rId))
                        {
                            d.Responses[rId].visibleIn[i] = 1;
                            continue;
                        }
                    }

                    for (byte i2 = 0; i2 < b; i2++)
                    {
                        byte? m;
                        if (asset.Has($"Response_{rId}_Message_{i2}"))
                            m = asset.ReadByte($"Response_{rId}_Message_{i2}");
                        else
                            m = null;
                        if (m != null)
                            d.Responses[rId].visibleIn[m.Value] = 1;
                    }
                }
                d.Responses[rId].mainText = local?.ReadString($"Response_{rId}");
                if (d.Responses[rId].mainText == null)
                {
                    break;
                }

                d.Responses[rId].openDialogueId = asset.ReadUInt16($"Response_{rId}_Dialogue");
                d.Responses[rId].openQuestId = asset.ReadUInt16($"Response_{rId}_Quest");
                d.Responses[rId].openVendorId = asset.ReadUInt16($"Response_{rId}_Vendor");
                d.Responses[rId].conditions = ParseConditions($"Response_{rId}_").ToLimitedList(byte.MaxValue);
                d.Responses[rId].rewards = ParseRewards($"Response_{rId}_").ToLimitedList(byte.MaxValue);
            }
            return d;
        }
        public NPCVendor ParseVendor()
        {
            return new NPCVendor()
            {
                ID = asset.ReadUInt16("ID"),
                disableSorting = asset.Has("Disable_Sorting"),
                GUID = asset.ReadGUID("GUID", Guid.NewGuid()).ToString("N"),
                Title = local?.ReadString("Name") ?? "",
                vendorDescription = local?.ReadString("Description") ?? "",
                items = ParseVendorItems().ToList(),
                currency = asset.Has("Currency") ? asset.ReadString("Currency") : ""
            };
        }
        public NPCQuest ParseQuest()
        {
            NPCQuest q = new NPCQuest()
            {
                ID = asset.ReadUInt16("ID"),
                Title = local?.ReadString("Name") ?? "",
                description = local?.ReadString("Description") ?? "",
                GUID = asset.ReadGUID("GUID", Guid.NewGuid()).ToString("N"),
                conditions = ParseConditions("").ToLimitedList(byte.MaxValue),
                rewards = ParseRewards("").ToLimitedList(byte.MaxValue)
            };
            return q;
        }
        private VendorItem[] ParseVendorItems()
        {
            byte buyAmount = asset.ReadByte("Buying");
            byte sellAmount = asset.ReadByte("Selling");
            List<VendorItem> items = new List<VendorItem>(buyAmount + sellAmount);
            for (byte i = 0; i < buyAmount; i++)
            {
                items.Add(new VendorItem()
                {
                    id = asset.ReadUInt16($"Buying_{i}_ID"),
                    cost = asset.ReadUInt16($"Buying_{i}_Cost"),
                    conditions = ParseConditions($"Buying_{i}_").ToList(),
                    isBuy = true
                });
            }
            for (byte i = 0; i < sellAmount; i++)
            {
                VendorItem vi = new VendorItem() { isBuy = false };
                string text = null;
                if (asset.Has($"Selling_{i}_Type"))
                {
                    text = asset.ReadString($"Selling_{i}_Type");
                }

                vi.id = asset.ReadUInt16($"Selling_{i}_ID");
                vi.cost = asset.ReadUInt32($"Selling_{i}_Cost");
                vi.conditions = ParseConditions($"Selling_{i}_").ToList();
                if (text == null || (text.Equals("Item", StringComparison.InvariantCultureIgnoreCase)))
                {
                    vi.type = ItemType.ITEM;
                    items.Add(vi);
                }
                else
                {
                    if (!text.Equals("Vehicle", StringComparison.InvariantCultureIgnoreCase))
                    {
                        App.Logger.Log($"Unknown VendorItem type '{text}'");
                    }
                    vi.type = ItemType.VEHICLE;
                    vi.spawnPointID = asset.ReadString($"Selling_{i}_Spawnpoint");
                    if (string.IsNullOrEmpty(vi.spawnPointID))
                    {
                        App.Logger.Log($"Selling Vehicle without Spawnpoint");
                    }
                    items.Add(vi);
                }
            }
            return items.ToArray();
        }
        private NPCClothing ParseClothing(Clothing_Type clothing)
        {
            NPCClothing c = new NPCClothing();
            string prefix = "";
            switch (clothing)
            {
                case Clothing_Type.Christmas:
                    if (!asset.Has("Has_Christmas_Outfit"))
                    {
                        return c;
                    }

                    prefix = "Christmas_";
                    break;
                case Clothing_Type.Halloween:
                    if (!asset.Has("Has_Halloween_Outfit"))
                    {
                        return c;
                    }

                    prefix = "Halloween_";
                    break;
            }
            c.Backpack = asset.ReadUInt16(prefix + "Backpack");
            c.Glasses = asset.ReadUInt16(prefix + "Glasses");
            c.Hat = asset.ReadUInt16(prefix + "Hat");
            c.Mask = asset.ReadUInt16(prefix + "Mask");
            c.Pants = asset.ReadUInt16(prefix + "Pants");
            c.Shirt = asset.ReadUInt16(prefix + "Shirt");
            c.Vest = asset.ReadUInt16(prefix + "Vest");
            return c;
        }
        private Condition[] ParseConditions(string prefix, string postfix = "Condition_")
        {
            Condition[] c = new Condition[asset.ReadByte(prefix + "Conditions")];

            int num = 0;
            string text;
            while (true)
            {
                if (num >= c.Length)
                {
                    return c;
                }

                text = $"{prefix}{postfix}{num}_Type";
                if (!asset.Has(text))
                {
                    break;
                }

                Condition_Type type = asset.ReadEnum(text, Condition_Type.None);
                string desc = local?.ReadString($"{prefix}{postfix}{num}");
                bool needToReset = asset.Has($"{prefix}{postfix}{num}_Reset");
                Logic_Type logic = asset.ReadEnum($"{prefix}{postfix}{num}_Logic", Logic_Type.Equal);
                string tp = $"{prefix}{postfix}{num}_";
                switch (type)
                {
                    case Condition_Type.Kills_Tree:
                        c[num] = new ConditionKillsTree()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Tree = asset.ReadString(tp + "Tree"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Currency:
                        c[num] = new ConditionCurrency()
                        {
                            GUID = asset.ReadString(tp + "GUID"),
                            Logic = logic,
                            Value = asset.ReadUInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Experience:
                        c[num] = new ConditionExperience()
                        {
                            Logic = logic,
                            Value = asset.ReadUInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Reputation:
                        c[num] = new ConditionReputation()
                        {
                            Logic = logic,
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Flag_Bool:
                        c[num] = new ConditionFlagBool()
                        {
                            Logic = logic,
                            Allow_Unset = asset.Has(tp + "Allow_Unset"),
                            ID = asset.ReadUInt16(tp + "ID"),
                            Value = asset.ReadBoolean(tp + "Value")
                        };
                        break;
                    case Condition_Type.Flag_Short:
                        c[num] = new ConditionFlagShort()
                        {
                            Logic = logic,
                            Allow_Unset = asset.Has(tp + "Allow_Unset"),
                            ID = asset.ReadUInt16(tp + "ID"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Quest:
                        c[num] = new ConditionQuest()
                        {
                            Logic = logic,
                            ID = asset.ReadUInt16(tp + "ID"),
                            Status = asset.ReadEnum<Quest_Status>(tp + "Status"),
                            Ignore_NPC = asset.Has(tp + "Ignore_NPC")
                        };
                        break;
                    case Condition_Type.Skillset:
                        c[num] = new ConditionSkillset()
                        {
                            Logic = logic,
                            Value = asset.ReadEnum<ESkillset>(tp + "Value")
                        };
                        break;
                    case Condition_Type.Item:
                        c[num] = new ConditionItem()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Amount = asset.ReadUInt16(tp + "Amount")
                        };
                        break;
                    case Condition_Type.Kills_Zombie:
                        c[num] = new ConditionKillsZombie()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Spawn = asset.Has(tp + "Spawn"),
                            Spawn_Quantity = asset.Has(tp + "Spawn_Quantity") ? asset.ReadInt32(tp + "Spawn_Quantity") : 1,
                            Value = asset.ReadInt16(tp + "Value"),
                            Radius = asset.ReadSingle(tp + "Radius", 512f),
                            Zombie = asset.ReadEnum<Zombie_Type>(tp + "Zombie")
                        };
                        break;
                    case Condition_Type.Kills_Horde:
                        c[num] = new ConditionKillsHorde()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Kills_Animal:
                        c[num] = new ConditionKillsAnimal()
                        {
                            Animal = asset.ReadUInt16(tp + "Animal"),
                            ID = asset.ReadUInt16(tp + "ID"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Compare_Flags:
                        c[num] = new ConditionCompareFlags()
                        {
                            Allow_A_Unset = asset.Has(tp + "Allow_A_Unset"),
                            Allow_B_Unset = asset.Has(tp + "Allow_B_Unset"),
                            A_ID = asset.ReadUInt16(tp + "A_ID"),
                            B_ID = asset.ReadUInt16(tp + "B_ID"),
                            Logic = logic
                        };
                        break;
                    case Condition_Type.Time_Of_Day:
                        c[num] = new ConditionTimeOfDay()
                        {
                            Logic = logic,
                            Second = asset.ReadInt32(tp + "Second")
                        };
                        break;
                    case Condition_Type.Player_Life_Health:
                        c[num] = new ConditionPlayerLifeHealth()
                        {
                            Logic = logic,
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Player_Life_Food:
                        c[num] = new ConditionPlayerLifeFood()
                        {
                            Logic = logic,
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Player_Life_Water:
                        c[num] = new ConditionPlayerLifeWater()
                        {
                            Logic = logic,
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Player_Life_Virus:
                        c[num] = new ConditionPlayerLifeVirus()
                        {
                            Logic = logic,
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Holiday:
                        c[num] = new ConditionHoliday()
                        {
                            Value = asset.ReadEnum<ENPCHoliday>(tp + "Value")
                        };
                        break;
                    case Condition_Type.Kills_Player:
                        c[num] = new ConditionKillsPlayer()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Kills_Object:
                        c[num] = new ConditionKillsObject()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Object = asset.ReadString(tp + "Object") ?? "",
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Weather_Blend_Alpha:
                        c[num] = new ConditionWeatherBlendAlpha()
                        {
                            GUID = asset.ReadString(tp + "GUID"),
                            Value = asset.ReadSingle(tp + "Value")
                        };
                        break;
                    case Condition_Type.Weather_Status:
                        c[num] = new ConditionWeatherStatus()
                        {
                            GUID = asset.ReadString(tp + "GUID"),
                            Value = asset.ReadEnum(tp + "Value", ENPCWeatherStatus.Active)
                        };
                        break;
                }
                c[num].Localization = desc ?? "";
                c[num].Reset = needToReset;
                num++;
            }
            return c;
        }
        private Reward[] ParseRewards(string prefix, string postfix = "Reward_")
        {
            Reward[] r = new Reward[asset.ReadByte($"{prefix}Rewards")];
            int num = 0;
            string text;
            while (true)
            {
                if (num >= r.Length)
                {
                    return r;
                }

                text = $"{prefix}{postfix}{num}_Type";
                if (!asset.Has(text))
                {
                    break;
                }

                RewardType type = asset.ReadEnum<RewardType>(text);
                string desc = local?.ReadString($"{prefix}{postfix}{num}");
                string tp = $"{prefix}{postfix}{num}_";
                switch (type)
                {
                    case RewardType.Achievement:
                        r[num] = new RewardAchievement()
                        {
                            ID = asset.ReadString(tp + "ID")
                        };
                        break;
                    case RewardType.Event:
                        r[num] = new RewardEvent()
                        {
                            ID = asset.ReadString(tp + "ID")
                        };
                        break;
                    case RewardType.Experience:
                        r[num] = new RewardExperience()
                        {
                            Value = asset.ReadUInt32(tp + "Value")
                        };
                        break;
                    case RewardType.Flag_Bool:
                        r[num] = new RewardFlagBool()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Value = asset.ReadBoolean(tp + "Value")
                        };
                        break;
                    case RewardType.Flag_Math:
                        r[num] = new RewardFlagMath()
                        {
                            A_ID = asset.ReadUInt16(tp + "A_ID"),
                            B_ID = asset.ReadUInt16(tp + "B_ID"),
                            Operation = asset.ReadEnum<Operation_Type>(tp + "Operation")
                        };
                        break;
                    case RewardType.Flag_Short:
                        r[num] = new RewardFlagShort()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Modification = asset.ReadEnum<Modification_Type>(tp + "Modification"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case RewardType.Flag_Short_Random:
                        r[num] = new RewardFlagShortRandom()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Max_Value = asset.ReadInt16(tp + "Max_Value"),
                            Min_Value = asset.ReadInt16(tp + "Min_Value"),
                            Modification = asset.ReadEnum<Modification_Type>(tp + "Modification")
                        };
                        break;
                    case RewardType.Item:
                        r[num] = new RewardItem()
                        {
                            Ammo = asset.ReadByte(tp + "Ammo"),
                            Amount = asset.ReadByte(tp + "Amount"),
                            Auto_Equip = asset.ReadBoolean(tp + "Auto_Equip"),
                            Barrel = asset.ReadUInt16(tp + "Barrel"),
                            Grip = asset.ReadUInt16(tp + "Grip"),
                            Magazine = asset.ReadUInt16(tp + "Magazine"),
                            Sight = asset.ReadUInt16(tp + "Sight"),
                            Tactical = asset.ReadUInt16(tp + "Tactical"),
                            ID = asset.ReadUInt16(tp + "ID")
                        };
                        break;
                    case RewardType.Item_Random:
                        r[num] = new RewardItemRandom()
                        {
                            Amount = asset.ReadByte(tp + "Amount"),
                            ID = asset.ReadUInt16(tp + "ID")
                        };
                        break;
                    case RewardType.Quest:
                        r[num] = new RewardQuest()
                        {
                            ID = asset.ReadUInt16(tp + "ID")
                        };
                        break;
                    case RewardType.Reputation:
                        r[num] = new RewardReputation()
                        {
                            Value = asset.ReadInt32(tp + "Value")
                        };
                        break;
                    case RewardType.Teleport:
                        r[num] = new RewardTeleport()
                        {
                            Spawnpoint = asset.ReadString(tp + "Spawnpoint")
                        };
                        break;
                    case RewardType.Vehicle:
                        r[num] = new RewardVehicle()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Spawnpoint = asset.ReadString(tp + "Spawnpoint")
                        };
                        break;
                    case RewardType.Currency:
                        r[num] = new RewardCurrency()
                        {
                            GUID = asset.ReadString(tp + "GUID"),
                            Value = asset.ReadUInt32(tp + "Value")
                        };
                        break;
                    case RewardType.Hint:
                        r[num] = new RewardHint()
                        {
                            Duration = asset.ReadSingle(tp + "Duration", 2f)
                        };
                        break;
                }
                r[num].Localization = desc ?? "";
                num++;
            }
            return r;
        }
        public ParseType GetParseType()
        {
            return asset.ReadEnum("Type", ParseType.None);
        }
    }
}
