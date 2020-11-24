using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableByteReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            byte.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out byte result);
            return result;
        }
    }
}
