using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableUIntReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            uint.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out uint result);
            return result;
        }
    }
}
