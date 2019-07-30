using Newtonsoft.Json;
using System.IO;

namespace BowieD.NPCMaker.Data
{
    public abstract class JsonConfig : IConfig
    {
        [JsonIgnore]
        public virtual string FileName => PathUtil.GetWorkDir() + "config.json";
        public virtual void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                JsonConvert.PopulateObject(File.ReadAllText(filePath), this);
            }
            else
            {
                LoadDefaults();
                Save(filePath);
            }
        }

        public virtual void Save(string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
        }

        public virtual void LoadDefaults() { }
    }
}
