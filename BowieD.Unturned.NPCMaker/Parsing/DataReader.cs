using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public sealed class DataReader
    {
        private readonly Dictionary<string, string> data;
        public DataReader(string content, bool overrideOldData = false)
        {
            data = new Dictionary<string, string>();
            StringReader reader = null;
            string line = "";
            try
            {
                reader = new StringReader(content);
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
                            if (overrideOldData)
                            {
                                data[text] = value;
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
                App.Logger.LogException("Failed to load data", ex: ex);
                data.Clear();
            }
            finally
            {
                reader?.Close();
            }
        }
        public bool Has(string key)
        {
            return data.ContainsKey(key);
        }

        public string ReadString(string key, string defaultValue = null)
        {
            if (!data.TryGetValue(key, out string res))
            {
                return defaultValue;
            }

            return res;
        }
        public T ReadEnum<T>(string key, T defaultValue = default) where T : struct
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
            {
                return new Coloring.Color(ReadString(key));
            }

            return defaultColor;
        }
        public Guid ReadGUID(string key)
        {
            return new Guid(ReadString(key));
        }
        public Guid ReadGUID(string key, Guid defaultValue)
        {
            if (Guid.TryParse(ReadString(key), out var _guid))
            {
                return _guid;
            }
            return defaultValue;
        }

        public GUIDIDBridge ReadGUIDIDBridge(string key)
        {
            GUIDIDBridge bridge = new GUIDIDBridge();

            if (data.TryGetValue(key, out var value))
            {
                bridge = GUIDIDBridge.Parse(value);
            }

            return bridge;
        }
    }
}
