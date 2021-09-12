namespace BowieD.Unturned.NPCMaker.Parsing.KVTable.TReaders.UnityTypes
{
    public struct Vector2
    {
        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public static implicit operator System.Windows.Point(Vector2 v2)
        {
            return new System.Windows.Point(v2.X, v2.Y);
        }
    }
    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    public class KVTableVector2Reader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            reader = reader.readObject();
            if (reader == null)
                return null;
            return new Vector2(reader.readValue<float>("X"), reader.readValue<float>("Y"));
        }
    }
    public class KVTableVector3Reader : ITypeReader
    {
        public object read(IFileReader reader)
        {
            reader = reader.readObject();
            if (reader == null)
                return null;
            return new Vector3(reader.readValue<float>("X"), reader.readValue<float>("Y"), reader.readValue<float>("Z"));
        }
    }
}
