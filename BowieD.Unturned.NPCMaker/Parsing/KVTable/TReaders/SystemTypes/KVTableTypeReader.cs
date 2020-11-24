using System;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.SystemTypes
{
    public class KVTableTypeReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            string text = reader.readValue();
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            text = KVTableTypeRedirectorRegistry.chase(text);
            return Type.GetType(text);
        }
    }
}
