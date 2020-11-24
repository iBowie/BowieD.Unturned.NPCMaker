using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableIntReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            int.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out int result);
            return result;
        }
    }
}
