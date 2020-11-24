using System;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.SystemTypes
{
    public class KVTableGUIDReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            string text = reader.readValue();
            if (string.IsNullOrEmpty(text) || text.Equals("0") || !Guid.TryParse(text, out var result))
            {
                return Guid.Empty;
            }
            return result;
        }
    }
}
