namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.CoreTypes
{
    public class KVTableStringReader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            return reader.readValue();
        }
    }
}
