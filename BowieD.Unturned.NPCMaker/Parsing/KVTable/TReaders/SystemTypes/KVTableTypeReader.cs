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

            if (text.StartsWith("SDG.Unturned.") || text.StartsWith("SDG.Framework."))
                return null;

            return Type.GetType(text);
        }
    }
}
