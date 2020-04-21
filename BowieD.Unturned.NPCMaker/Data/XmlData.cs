using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class XmlData<T> : IData<T>
    {
        protected XmlSerializer _serializer;
        protected XmlData()
        {
            _serializer = new XmlSerializer(typeof(T));
        }
        public virtual bool Indent => false;
        public abstract string FileName { get; }
        public T data;

        public event DataLoaded<T> OnDataLoaded;
        public event DataSaved<T> OnDataSaved;

        public virtual bool Save()
        {
            App.Logger.Log($"[XDATA] - Saving {FileName}");
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = Indent, IndentChars = "\t" }))
            {
                _serializer.Serialize(writer, data);
                App.Logger.Log($"[XDATA] - Saved!");
                OnDataSaved?.Invoke();
                return true;
            }
        }
        public virtual bool Load(T defaultValue)
        {
            App.Logger.Log($"[XDATA] - Loading {FileName}!");
            if (File.Exists(FileName))
            {
                App.Logger.Log($"[XDATA] - Converting from XML...");
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    if (_serializer.CanDeserialize(reader))
                    {
                        data = (T)_serializer.Deserialize(reader);
                        App.Logger.Log($"[XDATA] - Loaded");
                        OnDataLoaded?.Invoke();
                        return true;
                    }
                    else
                    {
                        App.Logger.Log($"[XDATA] - Could not load {FileName}. Reverting to default value...");
                        data = defaultValue;
                        Save();
                        return false;
                    }
                }
            }
            else
            {
                App.Logger.Log($"[XDATA] - {FileName} does not exist. Creating one with default value...");
                data = defaultValue;
                Save();
                return false;
            }
        }
    }
}
