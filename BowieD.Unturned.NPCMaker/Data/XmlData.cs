using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class XmlData<T> : IData<T>
    {
        private XmlSerializer _serializer;
        protected XmlData()
        {
            _serializer = new XmlSerializer(typeof(T));
        }
        public virtual bool Indent => false;
        public abstract string FileName { get; }
        public T data;
        public virtual void Save()
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = Indent, IndentChars = "\t" }))
            {
                _serializer.Serialize(writer, data);
            }
        }
        public virtual void Load(T defaultValue)
        {
            if (File.Exists(FileName))
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    if (_serializer.CanDeserialize(reader))
                        data = (T)_serializer.Deserialize(reader);
                    else
                    {
                        data = defaultValue;
                        Save();
                    }
                }
            }
            else
            {
                data = defaultValue;
                Save();
            }
        }
    }
}
