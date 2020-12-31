using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableUShortReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            ushort.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out ushort result);
            return result;
        }
    }
}
