using System.Globalization;

namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableFloatReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            float.TryParse(reader.readValue(), NumberStyles.Any, CultureInfo.InvariantCulture, out float result);
            return result;
        }
    }
}
