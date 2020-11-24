using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public class KVTableReader : IFileReader
    {
        protected static StringBuilder builder = new StringBuilder();

        protected string key;

        protected int index;

        protected string dictionaryKey;

        protected bool dictionaryInQuotes;

        protected bool dictionaryIgnoreNextChar;

        protected bool listInQuotes;

        protected bool listIgnoreNextChar;

        public Dictionary<string, object> table
        {
            get;
            protected set;
        }

        public virtual IEnumerable<string> getKeys()
        {
            return table.Keys;
        }

        public virtual bool containsKey(string key)
        {
            return table.ContainsKey(key);
        }

        public virtual void readKey(string key)
        {
            this.key = key;
            index = -1;
        }

        public virtual int readArrayLength(string key)
        {
            readKey(key);
            return readArrayLength();
        }

        public virtual int readArrayLength()
        {
            if (table.TryGetValue(key, out object value))
            {
                return (value as List<object>).Count;
            }
            return 0;
        }

        public virtual void readArrayIndex(string key, int index)
        {
            readKey(key);
            readArrayIndex(index);
        }

        public virtual void readArrayIndex(int index)
        {
            this.index = index;
        }

        public virtual string readValue(string key)
        {
            readKey(key);
            return readValue();
        }

        public virtual string readValue(int index)
        {
            readArrayIndex(index);
            return readValue();
        }

        public virtual string readValue(string key, int index)
        {
            readKey(key);
            readArrayIndex(index);
            return readValue();
        }

        public virtual string readValue()
        {
            if (index == -1)
            {
                if (!table.TryGetValue(key, out object value))
                {
                    return null;
                }
                return value.ToString();
            }
            if (table.TryGetValue(key, out object value2))
            {
                return (value2 as List<object>)[index].ToString();
            }
            return null;
        }

        public virtual object readValue(Type type, string key)
        {
            readKey(key);
            return readValue(type);
        }

        public virtual object readValue(Type type, int index)
        {
            readArrayIndex(index);
            return readValue(type);
        }

        public virtual object readValue(Type type, string key, int index)
        {
            readKey(key);
            readArrayIndex(index);
            return readValue(type);
        }

        public virtual object readValue(Type type)
        {
            if (typeof(IFileReadable).IsAssignableFrom(type))
            {
                IFileReadable obj = Activator.CreateInstance(type) as IFileReadable;
                obj.read(this);
                return obj;
            }
            return KVTableTypeReaderRegistry.read(this, type);
        }

        public virtual T readValue<T>(string key)
        {
            readKey(key);
            return readValue<T>();
        }

        public virtual T readValue<T>(int index)
        {
            readArrayIndex(index);
            return readValue<T>();
        }

        public virtual T readValue<T>(string key, int index)
        {
            readKey(key);
            readArrayIndex(index);
            return readValue<T>();
        }

        public virtual T readValue<T>()
        {
            if (typeof(IFileReadable).IsAssignableFrom(typeof(T)))
            {
                IFileReadable obj = Activator.CreateInstance<T>() as IFileReadable;
                obj.read(this);
                return (T)obj;
            }
            return KVTableTypeReaderRegistry.read<T>(this);
        }

        public virtual IFileReader readObject(string key)
        {
            readKey(key);
            return readObject();
        }

        public virtual IFileReader readObject(int index)
        {
            readArrayIndex(index);
            return readObject();
        }

        public virtual IFileReader readObject(string key, int index)
        {
            readKey(key);
            readArrayIndex(index);
            return readObject();
        }

        public virtual IFileReader readObject()
        {
            if (index == -1)
            {
                if (table.TryGetValue(key, out object value))
                {
                    return value as IFileReader;
                }
                return null;
            }
            if (table.TryGetValue(key, out object value2))
            {
                return (value2 as List<object>)[index] as IFileReader;
            }
            return null;
        }

        protected virtual bool canContinueReadDictionary(StreamReader input, Dictionary<string, object> scope)
        {
            return true;
        }

        public virtual void readDictionary(StreamReader input, Dictionary<string, object> scope)
        {
            dictionaryKey = null;
            dictionaryInQuotes = false;
            dictionaryIgnoreNextChar = false;
            while (!input.EndOfStream)
            {
                char c = (char)input.Read();
                if (dictionaryIgnoreNextChar)
                {
                    builder.Append(c);
                    dictionaryIgnoreNextChar = false;
                    continue;
                }
                switch (c)
                {
                    case '\\':
                        dictionaryIgnoreNextChar = true;
                        continue;
                    case '"':
                        if (dictionaryInQuotes)
                        {
                            dictionaryInQuotes = false;
                            if (string.IsNullOrEmpty(dictionaryKey))
                            {
                                dictionaryKey = builder.ToString();
                                continue;
                            }
                            string value = builder.ToString();
                            if (!scope.ContainsKey(dictionaryKey))
                            {
                                scope.Add(dictionaryKey, value);
                            }
                            if (!canContinueReadDictionary(input, scope))
                            {
                                return;
                            }
                            dictionaryKey = null;
                        }
                        else
                        {
                            dictionaryInQuotes = true;
                            builder.Length = 0;
                        }
                        continue;
                }
                if (dictionaryInQuotes)
                {
                    builder.Append(c);
                    continue;
                }
                switch (c)
                {
                    case '}':
                        return;
                    case '{':
                        {
                            if (scope.TryGetValue(dictionaryKey, out object value3))
                            {
                                KVTableReader keyValueTableReader = (KVTableReader)value3;
                                keyValueTableReader.readDictionary(input, keyValueTableReader.table);
                            }
                            else
                            {
                                KVTableReader keyValueTableReader2 = new KVTableReader(input);
                                value3 = keyValueTableReader2;
                                scope.Add(dictionaryKey, keyValueTableReader2);
                            }
                            if (!canContinueReadDictionary(input, scope))
                            {
                                return;
                            }
                            dictionaryKey = null;
                            break;
                        }
                    case '[':
                        {
                            if (!scope.TryGetValue(dictionaryKey, out object value2))
                            {
                                value2 = new List<object>();
                                scope.Add(dictionaryKey, value2);
                            }
                            readList(input, (List<object>)value2);
                            if (!canContinueReadDictionary(input, scope))
                            {
                                return;
                            }
                            dictionaryKey = null;
                            break;
                        }
                }
            }
        }

        public virtual void readList(StreamReader input, List<object> scope)
        {
            listInQuotes = false;
            listIgnoreNextChar = false;
            while (!input.EndOfStream)
            {
                char c = (char)input.Read();
                if (listIgnoreNextChar)
                {
                    builder.Append(c);
                    listIgnoreNextChar = false;
                    continue;
                }
                switch (c)
                {
                    case '\\':
                        listIgnoreNextChar = true;
                        continue;
                    case '"':
                        if (listInQuotes)
                        {
                            listInQuotes = false;
                            string item = builder.ToString();
                            scope.Add(item);
                        }
                        else
                        {
                            listInQuotes = true;
                            builder.Length = 0;
                        }
                        continue;
                }
                if (listInQuotes)
                {
                    builder.Append(c);
                    continue;
                }
                switch (c)
                {
                    case ']':
                        return;
                    case '{':
                        {
                            KVTableReader item2 = new KVTableReader(input);
                            scope.Add(item2);
                            break;
                        }
                }
            }
        }

        public KVTableReader()
        {
            table = new Dictionary<string, object>();
        }

        public KVTableReader(StreamReader input)
        {
            table = new Dictionary<string, object>();
            readDictionary(input, table);
        }
    }
}
