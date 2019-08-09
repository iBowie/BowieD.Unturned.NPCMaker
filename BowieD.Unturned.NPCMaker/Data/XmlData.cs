using BowieD.Unturned.NPCMaker.Logging;
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
            App.Logger.LogInfo($"[XDATA] - Saving {FileName}");
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = Indent, IndentChars = "\t" }))
            {
                _serializer.Serialize(writer, data);
                App.Logger.LogInfo($"[XDATA] - Saved!");
            }
        }
        public virtual void Load(T defaultValue)
        {
            App.Logger.LogInfo($"[XDATA] - Loading {FileName}!");
            if (File.Exists(FileName))
            {
                App.Logger.LogInfo($"[XDATA] - Converting from XML...");
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    if (_serializer.CanDeserialize(reader))
                    {
                        data = (T)_serializer.Deserialize(reader);
                        App.Logger.LogInfo($"[XDATA] - Loaded");
                    }
                    else
                    {
                        App.Logger.LogInfo($"[XDATA] - Could not load {FileName}. Reverting to default value...");
                        data = defaultValue;
                        Save();
                    }
                }
            }
            else
            {
                App.Logger.LogInfo($"[XDATA] - {FileName} does not exist. Creating one with default value...");
                data = defaultValue;
                Save();
            }
        }
    }
}
