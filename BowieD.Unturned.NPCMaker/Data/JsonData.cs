using Newtonsoft.Json;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class JsonData<T> : IData<T>
    {
        [JsonIgnore]
        public abstract string FileName { get; }
        public T data;
        public virtual void Save()
        {
            string content = JsonConvert.SerializeObject(data);
            File.WriteAllText(FileName, content);
        }
        public virtual void Load(T defaultValue)
        {
            if (File.Exists(FileName))
            {
                try
                {
                    string content = File.ReadAllText(FileName);
                    data = JsonConvert.DeserializeObject<T>(content);
                }
                catch
                {
                    data = defaultValue;
                    Save();
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
