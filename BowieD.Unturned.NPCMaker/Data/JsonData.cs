using Newtonsoft.Json;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class JsonData<T> : IData<T>
    {
        [JsonIgnore]
        public abstract string FileName { get; }
        public T data;
        public virtual bool Save()
        {
            App.Logger.Log($"[JDATA] - Saving {FileName}");
            string content = JsonConvert.SerializeObject(data);
            File.WriteAllText(FileName, content);
            App.Logger.Log($"[JDATA] - Saved!");
            return true;
        }
        public virtual bool Load(T defaultValue)
        {
            App.Logger.Log($"[JDATA] - Loading {FileName}");
            if (File.Exists(FileName))
            {
                try
                {
                    App.Logger.Log($"[JDATA] - Converting from JSON...");
                    string content = File.ReadAllText(FileName);
                    data = JsonConvert.DeserializeObject<T>(content);
                    App.Logger.Log($"[JDATA] - Loaded");
                    return true;
                }
                catch
                {
                    App.Logger.Log($"[JDATA] - Could not load {FileName}. Reverting to default value...", Logging.ELogLevel.WARNING);
                    data = defaultValue;
                    Save();
                    return false;
                }
            }
            else
            {
                App.Logger.Log($"[JDATA] - {FileName} does not exist. Creating one with default value...", Logging.ELogLevel.WARNING);
                data = defaultValue;
                Save();
                return false;
            }
        }
    }
}
