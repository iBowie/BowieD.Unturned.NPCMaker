using System;
using System.IO;
using System.Runtime.Serialization;

namespace BowieD.Unturned.NPCMaker.Common
{
    public static class CloneHelper
    {
        private static IFormatter _formatter;
        private static IFormatter Formatter
        {
            get
            {
                if (_formatter is null)
                {
                    _formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                }
                return _formatter;
            }
        }
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("Presented type is not serializable");

            if (Object.ReferenceEquals(source, null))
                return default;


            Stream stream = new MemoryStream();
            using (stream)
            {
                Formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)Formatter.Deserialize(stream);
            }
        }
    }
}
