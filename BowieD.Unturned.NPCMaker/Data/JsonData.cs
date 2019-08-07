using BowieD.Unturned.NPCMaker.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class JsonData<T>
    {
        public T data;
        public abstract void Save();
        public abstract void Load();
    }
    public sealed class RecentFileList : JsonData<string[]>
    {
        private static string path => AppConfig.Directory + "recent.json";
        public override void Load()
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<string[]>(content);
            }
            else
            {
                data = new string[0];
                Save();
            }
        }
        public override void Save()
        {
            string content = JsonConvert.SerializeObject(data);
            File.WriteAllText(path, content);
        }
    }
    public sealed class UserColorsList : JsonData<string[]>
    {
        private static string path => AppConfig.Directory + "colors.json";
        public override void Load()
        {
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<string[]>(content);
            }
            else
            {
                data = new string[0];
                Save();
            }
        }
        public override void Save()
        {
            string content = JsonConvert.SerializeObject(data);
            File.WriteAllText(path, content);
        }
    }
}
