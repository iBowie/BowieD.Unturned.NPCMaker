using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public static class KVTableTypeReaderRegistry
    {
        static KVTableTypeReaderRegistry()
        {
            add<bool>(new KVTable.TReaders.CoreTypes.KVTableBoolReader());
            add<sbyte>(new KVTable.TReaders.CoreTypes.KVTableSByteReader());
            add<byte>(new KVTable.TReaders.CoreTypes.KVTableByteReader());
            add<short>(new KVTable.TReaders.CoreTypes.KVTableShortReader());
            add<int>(new KVTable.TReaders.CoreTypes.KVTableIntReader());
            add<long>(new KVTable.TReaders.CoreTypes.KVTableLongReader());
            add<ushort>(new KVTable.TReaders.CoreTypes.KVTableUShortReader());
            add<uint>(new KVTable.TReaders.CoreTypes.KVTableUIntReader());
            add<ulong>(new KVTable.TReaders.CoreTypes.KVTableULongReader());
            add<float>(new KVTable.TReaders.CoreTypes.KVTableFloatReader());
            add<string>(new KVTable.TReaders.CoreTypes.KVTableStringReader());

            add<Guid>(new KVTable.TReaders.SystemTypes.KVTableGUIDReader());
            add<Type>(new KVTable.TReaders.SystemTypes.KVTableTypeReader());
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
