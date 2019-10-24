using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public sealed partial class ParseTool : IDisposable
    {
        private Data local;
        private Data asset;
        private string dir;
        public ParseTool(string fileName)
        {
            dir = Path.GetDirectoryName(fileName);
            asset = new Data(File.ReadAllText(fileName));
            if (File.Exists(dir + "English.dat"))
                local = new Data(File.ReadAllText(fileName));
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
                visibilityConditions = ParseConditions("Condition_").ToList()
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
                        App.Logger.LogWarning($"Page {pId} in message {mId} not found.");
                    d.messages[mId].pages.Add(page);
                }
                d.messages[mId].conditions = ParseConditions($"Message_{mId}_Condition_");
            }
            d.responses = new List<NPCResponse>(asset.ReadByte("Responses"));
            for (byte rId = 0; rId < d.responses.Capacity; rId++)
            {
                byte b = asset.ReadByte($"Response_{rId}_Messages");
                d.responses[rId].visibleIn = new int[b];
                for (byte i = 0; i < b; i++)
                {
                    d.responses[rId].visibleIn[i] = asset.Has($"Response_{rId}_Message_{i}") ? 1 : 0;
                }
                d.responses.Add(new NPCResponse());
                d.responses[rId].mainText = local?.ReadString($"Response_{rId}");
                if (d.responses[rId].mainText == null)
                    break;
                d.responses[rId].openDialogueId = asset.ReadUInt16($"Response_{rId}_Dialogue");
                d.responses[rId].openQuestId = asset.ReadUInt16($"Response_{rId}_Quest");
                d.responses[rId].openVendorId = asset.ReadUInt16($"Response_{rId}_Vendor");
                d.responses[rId].conditions = ParseConditions($"Response_{rId}_Condition_");
                d.responses[rId].rewards = ParseRewards($"Response_{rId}_Reward_");
            }
            return d;
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
        private Condition[] ParseConditions(string prefix)
        {
            Condition[] c = new Condition[asset.ReadByte("Conditions")];

            int num = 0;
            string text;
            while (true)
            {
                if (num >= c.Length)
                    return c;
                text = prefix + num + "_Type";
                if (!asset.Has(text))
                    break;
                Condition_Type type = asset.ReadEnum(text, Condition_Type.None);
                string desc = local?.ReadString(prefix + num);
                bool needToReset = asset.ReadBoolean(prefix + num + "_Reset");
                Logic_Type logic = asset.ReadEnum(prefix + num + "_Logic", Logic_Type.Equal);
                string tp = prefix + num + "_";
                switch (type)
                {
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
                        break;
                    case Condition_Type.Kills_Animal:
                        break;
                    case Condition_Type.Compare_Flags:
                        break;
                    case Condition_Type.Time_Of_Day:
                        break;
                    case Condition_Type.Player_Life_Health:
                        break;
                    case Condition_Type.Player_Life_Food:
                        break;
                    case Condition_Type.Player_Life_Water:
                        break;
                    case Condition_Type.Player_Life_Virus:
                        break;
                    case Condition_Type.Holiday:
                        break;
                    case Condition_Type.Kills_Player:
                        break;
                    case Condition_Type.Kills_Object:
                        break;
                }
                c[num].Localization = desc;
            }
            return c;
        }
        private Reward[] ParseRewards(string prefix)
        {

        }
        public void Dispose()
        {
            asset.Dispose();
            local?.Dispose();
        }
        public ParseType GetParseType() => asset.ReadEnum("Type", ParseType.None);
    }
    public sealed class Data
    {
        private Dictionary<string, string> data;
        public Data(string content, bool overrideOldData = false)
        {
            data = new Dictionary<string, string>();
            StringReader reader = null;
            string line = "";
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (!(line == string.Empty) && (line.Length <= 1 || !(line.Substring(0, 2) == "//")))
                    {
                        int num = line.IndexOf(' ');
                        string text;
                        string value;
                        if (num != -1)
                        {
                            text = line.Substring(0, num);
                            value = line.Substring(num + 1, line.Length - num - 1);
                        }
                        else
                        {
                            text = line;
                            value = string.Empty;
                        }
                        if (data.ContainsKey(text))
                        {
                            App.Logger.LogWarning("Multiple instances of '" + text + "'");
                            if (overrideOldData)
                            {
                                App.Logger.LogWarning($"Overriding {text}...");
                            }
                        }
                        else
                        {
                            data.Add(text, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException("Failed to load data", ex);
                data.Clear();
            }
            finally
            {
                reader?.Close();
            }
        }
        public bool Has(string key) => data.ContainsKey(key);
        public string ReadString(string key, string defaultValue = null)
        {
            if (!data.TryGetValue(key, out string res))
                return defaultValue;
            return res;
        }
        public T ReadEnum<T>(string key, T defaultValue = default(T)) where T : struct
        {
            if (data.TryGetValue(key, out string value))
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        public bool ReadBoolean(string key, bool defaultValue = false)
        {
            if (data.TryGetValue(key, out string value))
            {
                return value.Equals("y", StringComparison.InvariantCultureIgnoreCase) || value == "1" || value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }
            return defaultValue;
        }

        public byte ReadByte(string key, byte defaultValue = 0)
        {
            if (!byte.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out byte result))
            {
                return defaultValue;
            }
            return result;
        }

        public byte[] ReadByteArray(string key)
        {
            string s = ReadString(key);
            return Encoding.UTF8.GetBytes(s);
        }

        public short ReadInt16(string key, short defaultValue = 0)
        {
            if (!short.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out short result))
            {
                return defaultValue;
            }
            return result;
        }

        public ushort ReadUInt16(string key, ushort defaultValue = 0)
        {
            if (!ushort.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out ushort result))
            {
                return defaultValue;
            }
            return result;
        }

        public int ReadInt32(string key, int defaultValue = 0)
        {
            if (!int.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out int result))
            {
                return defaultValue;
            }
            return result;
        }

        public uint ReadUInt32(string key, uint defaultValue = 0u)
        {
            if (!uint.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out uint result))
            {
                return defaultValue;
            }
            return result;
        }

        public long ReadInt64(string key, long defaultValue = 0L)
        {
            if (!long.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out long result))
            {
                return defaultValue;
            }
            return result;
        }

        public ulong ReadUInt64(string key, ulong defaultValue = 0uL)
        {
            if (!ulong.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out ulong result))
            {
                return defaultValue;
            }
            return result;
        }

        public float ReadSingle(string key, float defaultValue = 0f)
        {
            if (!float.TryParse(ReadString(key), NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
            {
                return defaultValue;
            }
            return result;
        }
        public Coloring.Color ReadColor(string key)
        {
            return ReadColor(key, new Coloring.Color(0, 0, 0));
        }

        public Coloring.Color ReadColor(string key, Coloring.Color defaultColor)
        {
            string d = ReadString(key);
            if (d != null)
                return new Coloring.Color(ReadString(key));
            return defaultColor;
        }
        public Guid ReadGUID(string key)
        {
            return new Guid(ReadString(key));
        }
    }
}
