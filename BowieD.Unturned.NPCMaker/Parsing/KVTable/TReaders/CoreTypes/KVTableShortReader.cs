using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableShortReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            short.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out short result);
            return result;
        }
    }
}
