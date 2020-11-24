using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableLongReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            long.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out long result);
            return result;
        }
    }
}
