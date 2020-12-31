using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableSByteReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            sbyte.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out sbyte result);
            return result;
        }
    }
}
