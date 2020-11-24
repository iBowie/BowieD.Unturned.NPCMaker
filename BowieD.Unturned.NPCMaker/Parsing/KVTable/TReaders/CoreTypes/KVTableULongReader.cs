using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableULongReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            ulong.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out ulong result);
            return result;
        }
    }
}
