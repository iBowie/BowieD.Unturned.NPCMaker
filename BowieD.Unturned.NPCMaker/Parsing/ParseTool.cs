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
        private DataReader local;
        private DataReader asset;
        private string dir;
        public ParseTool(string fileName)
        {
            dir = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;
            asset = new DataReader(File.ReadAllText(fileName));
            foreach (var k in Enum.GetValues(typeof(ELanguage)))
            {
                if (File.Exists(dir + "English.dat"))
                {
                    local = new DataReader(File.ReadAllText(dir + "English.dat"));
                    break;
                }
                else
                {
                    App.Logger.Log($"[ParseTool] - English.dat not found. Checking all languages...");
                    if (File.Exists(dir + $"{k}.dat"))
                    {
                        local = new DataReader(File.ReadAllText(dir + $"{k}.dat"));
                        break;
                    }
                    else
                        App.Logger.Log($"[ParseTool] - {k}.dat not found. Checking next...");
                }
            }
        }
        public NPCCharacter ParseCharacter()
        {
            return new NPCCharacter
            {
                id = asset.ReadUInt16("ID"),
                beard = asset.ReadByte("Beard"),
                face = asset.ReadByte("Face"),
                haircut = asset.ReadByte("Hair"),
                hairColor = asset.ReadColor("Color_Hair"),
                skinColor = asset.ReadColor("Color_Skin"),
                equipPrimary = asset.ReadUInt16("Primary"),
                equipSecondary = asset.ReadUInt16("Secondary"),
                equipTertiary = asset.ReadUInt16("Tertiary"),
                startDialogueId = asset.ReadUInt16("Dialogue"),
                displayName = local?.ReadString("Character"),
                editorName = local?.ReadString("Name"),
                guid = asset.Has("GUID") ? asset.ReadString("GUID") : Guid.NewGuid().ToString("N"),
                leftHanded = asset.Has("Backward"),
                clothing = ParseClothing(Clothing_Type.Default),
                christmasClothing = ParseClothing(Clothing_Type.Christmas),
                halloweenClothing = ParseClothing(Clothing_Type.Halloween),
                pose = asset.ReadEnum("Pose", NPC_Pose.Stand),
                equipped = asset.ReadEnum("Equipped", Equip_Type.None),
                visibilityConditions = ParseConditions("").ToList()
            };
        }
        public NPCDialogue ParseDialogue()
        {
            var d = new NPCDialogue()
            {
                guid = asset.Has("GUID") ? asset.ReadString("GUID") : Guid.NewGuid().ToString("N"),
                id = asset.ReadUInt16("ID")
            };
            d.messages = new List<NPCMessage>(asset.ReadByte("Messages"));
            for (byte mId = 0; mId < d.messages.Capacity; mId++)
            {
                d.messages.Add(new NPCMessage());
                d.messages[mId].pages = new List<string>(asset.ReadByte($"Message_{mId}_Pages"));
                for (byte pId = 0; pId < d.messages[mId].pages.Capacity; pId++)
                {
                    string page = local?.ReadString($"Message_{mId}_Page_{pId}");
                    if (page == null)
                        App.Logger.Log($"Page {pId} in message {mId} not found.");
                    d.messages[mId].pages.Add(page);
                }
                d.messages[mId].conditions = ParseConditions($"Message_{mId}_");
                d.messages[mId].rewards = ParseRewards($"Message_{mId}_");
            }
            d.responses = new List<NPCResponse>(asset.ReadByte("Responses"));
            for (byte rId = 0; rId < d.responses.Capacity; rId++)
            {
                d.responses.Add(new NPCResponse());
                byte b = asset.ReadByte($"Response_{rId}_Messages");
                d.responses[rId].visibleIn = new int[b];
                for (byte i = 0; i < b; i++)
                {
                    d.responses[rId].visibleIn[i] = asset.Has($"Response_{rId}_Message_{i}") ? 1 : 0;
                }
                d.responses[rId].mainText = local?.ReadString($"Response_{rId}");
                if (d.responses[rId].mainText == null)
                    break;
                d.responses[rId].openDialogueId = asset.ReadUInt16($"Response_{rId}_Dialogue");
                d.responses[rId].openQuestId = asset.ReadUInt16($"Response_{rId}_Quest");
                d.responses[rId].openVendorId = asset.ReadUInt16($"Response_{rId}_Vendor");
                d.responses[rId].conditions = ParseConditions($"Response_{rId}_");
                d.responses[rId].rewards = ParseRewards($"Response_{rId}_");
            }
            return d;
        }
        public NPCVendor ParseVendor()
        {
            return new NPCVendor()
            {
                id = asset.ReadUInt16("ID"),
                disableSorting = asset.Has("Disable_Sorting"),
                guid = asset.Has("GUID") ? asset.ReadString("GUID") : Guid.NewGuid().ToString("N"),
                vendorTitle = local?.ReadString("Name") ?? "",
                vendorDescription = local?.ReadString("Description") ?? "",
                items = ParseVendorItems().ToList(),
                currency = asset.Has("Currency") ? asset.ReadString("Currency") : ""
            };
        }
        public NPCQuest ParseQuest()
        {
            var q = new NPCQuest()
            {
                id = asset.ReadUInt16("ID"),
                title = local?.ReadString("Name") ?? "",
                description = local?.ReadString("Description") ?? "",
                guid = asset.Has("GUID") ? asset.ReadString("GUID") : Guid.NewGuid().ToString("N"),
                conditions = ParseConditions("").ToList(),
                rewards = ParseRewards("").ToList()
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
                    text = asset.ReadString($"Selling_{i}_Type");
                vi.id = asset.ReadUInt16($"Selling_{i}_ID");
                vi.cost = asset.ReadUInt16($"Selling_{i}_Cost");
                vi.conditions = ParseConditions($"Selling_{i}_").ToList();
                if (text == null || (text.Equals("Item", StringComparison.InvariantCultureIgnoreCase)))
                {
                    items.Add(vi);
                }
                else
                {
                    if (!text.Equals("Vehicle", StringComparison.InvariantCultureIgnoreCase))
                    {
                        App.Logger.Log($"Unknown VendorItem type '{text}'");
                    }
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
                        return c;
                    prefix = "Christmas_";
                    break;
                case Clothing_Type.Halloween:
                    if (!asset.Has("Has_Halloween_Outfit"))
                        return c;
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
                    return c;
                text = $"{prefix}{postfix}{num}_Type";
                if (!asset.Has(text))
                    break;
                Condition_Type type = asset.ReadEnum(text, Condition_Type.None);
                string desc = local?.ReadString($"{prefix}{postfix}{num}");
                bool needToReset = asset.ReadBoolean($"{prefix}{postfix}{num}_Reset");
                Logic_Type logic = asset.ReadEnum($"{prefix}{postfix}{num}_Logic", Logic_Type.Equal);
                string tp = $"{prefix}{postfix}{num}_";
                switch (type)
                {
                    case Condition_Type.Kills_Tree:
                        c[num] = new ConditionKillsTree()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Reset = needToReset,
                            Tree = asset.ReadString(tp + "Tree"),
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Currency:
                        c[num] = new ConditionCurrency()
                        {
                            GUID = asset.ReadString(tp + "GUID"),
                            Logic = logic,
                            Reset = needToReset,
                            Value = asset.ReadUInt32(tp + "Value")
                        };
                        break;
                    case Condition_Type.Experience:
                        c[num] = new ConditionExperience()
                        {
                            Logic = logic,
                            Reset = needToReset,
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
                            Reset = needToReset,
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
                            Reset = needToReset,
                            Status = asset.ReadEnum<Quest_Status>(tp + "Status")
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
                            Amount = asset.ReadUInt16(tp + "Amount"),
                            Reset = needToReset
                        };
                        break;
                    case Condition_Type.Kills_Zombie:
                        c[num] = new ConditionKillsZombie()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Reset = needToReset,
                            Spawn = asset.Has(tp + "Spawn"),
                            Spawn_Quantity = asset.Has(tp + "Spawn_Quantity") ? asset.ReadInt32(tp + "Spawn_Quantity") : 1,
                            Value = asset.ReadInt16(tp + "Value"),
                            Zombie = asset.ReadEnum<Zombie_Type>(tp + "Zombie")
                        };
                        break;
                    case Condition_Type.Kills_Horde:
                        c[num] = new ConditionKillsHorde()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Reset = needToReset,
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Kills_Animal:
                        c[num] = new ConditionKillsAnimal()
                        {
                            Animal = asset.ReadUInt16(tp + "Animal"),
                            ID = asset.ReadUInt16(tp + "ID"),
                            Reset = needToReset,
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
                            Logic = logic,
                            Reset = needToReset
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
                            Reset = needToReset,
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                    case Condition_Type.Kills_Object:
                        c[num] = new ConditionKillsObject()
                        {
                            ID = asset.ReadUInt16(tp + "ID"),
                            Nav = asset.ReadByte(tp + "Nav"),
                            Object = asset.ReadString(tp + "Object") ?? "",
                            Reset = needToReset,
                            Value = asset.ReadInt16(tp + "Value")
                        };
                        break;
                }
                c[num].Localization = desc ?? "";
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
                    return r;
                text = $"{prefix}{postfix}{num}_Type";
                if (!asset.Has(text))
                    break;
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
                            Duration = asset.ReadSingle(tp + "Duration", 2f),
                            Text = asset.ReadString(tp + "Text")
                        };
                        break;
                }
                r[num].Localization = desc ?? "";
                num++;
            }
            return r;
        }
        public ParseType GetParseType() => asset.ReadEnum("Type", ParseType.None);
    }
}
