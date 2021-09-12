using BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes;
using BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.SystemTypes;
using BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.UnityTypes;
using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public static class KVTableTypeReaderRegistry
    {
        static KVTableTypeReaderRegistry()
        {
            // Core
            add<bool>(new KVTableBoolReader());
            add<sbyte>(new KVTableSByteReader());
            add<byte>(new KVTableByteReader());
            add<short>(new KVTableShortReader());
            add<int>(new KVTableIntReader());
            add<long>(new KVTableLongReader());
            add<ushort>(new KVTableUShortReader());
            add<uint>(new KVTableUIntReader());
            add<ulong>(new KVTableULongReader());
            add<float>(new KVTableFloatReader());
            add<string>(new KVTableStringReader());
            // System
            add<Guid>(new KVTableGUIDReader());
            add<Type>(new KVTableTypeReader());
            // Unity
            add<Vector2>(new KVTableVector2Reader());
            add<Vector3>(new KVTableVector3Reader());
        }
        private static Dictionary<Type, ITypeReader> readers = new Dictionary<Type, ITypeReader>();

        public static T read<T>(IFileReader input)
        {
            if (readers.TryGetValue(typeof(T), out ITypeReader value))
            {
                object obj = value.read(input);
                if (obj == null)
                {
                    return default(T);
                }
                return (T)obj;
            }
            if (typeof(T).IsEnum)
            {
                string value2 = input.readValue();
                if (string.IsNullOrEmpty(value2))
                {
                    return default(T);
                }
                return (T)Enum.Parse(typeof(T), value2, ignoreCase: true);
            }
            return default(T);
        }

        public static object read(IFileReader input, Type type)
        {
            if (readers.TryGetValue(type, out ITypeReader value))
            {
                object obj = value.read(input);
                if (obj == null)
                {
                    if (type.IsValueType)
                        return Activator.CreateInstance(type);
                    else
                        return null;
                }
                return obj;
            }
            if (type.IsEnum)
            {
                string value2 = input.readValue();
                if (string.IsNullOrEmpty(value2))
                {
                    if (type.IsValueType)
                        return Activator.CreateInstance(type);
                    else
                        return null;
                }
                return Enum.Parse(type, value2, ignoreCase: true);
            }

            if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }

        public static void add<T>(ITypeReader reader)
        {
            add(typeof(T), reader);
        }

        public static void add(Type type, ITypeReader reader)
        {
            readers.Add(type, reader);
        }

        public static void remove<T>()
        {
            remove(typeof(T));
        }

        public static void remove(Type type)
        {
            readers.Remove(type);
        }
    }
}
