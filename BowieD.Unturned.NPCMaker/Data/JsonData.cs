﻿using BowieD.Unturned.NPCMaker.Logging;
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
            App.Logger.LogInfo($"[JDATA] - Saving {FileName}");
            string content = JsonConvert.SerializeObject(data);
            File.WriteAllText(FileName, content);
            App.Logger.LogInfo($"[JDATA] - Saved!");
            return true;
        }
        public virtual bool Load(T defaultValue)
        {
            App.Logger.LogInfo($"[JDATA] - Loading {FileName}");
            if (File.Exists(FileName))
            {
                try
                {
                    App.Logger.LogInfo($"[JDATA] - Converting from JSON...");
                    string content = File.ReadAllText(FileName);
                    data = JsonConvert.DeserializeObject<T>(content);
                    App.Logger.LogInfo($"[JDATA] - Loaded");
                    return true;
                }
                catch
                {
                    App.Logger.LogInfo($"[JDATA] - Could not load {FileName}. Reverting to default value...");
                    data = defaultValue;
                    Save();
                    return false;
                }
            }
            else
            {
                App.Logger.LogInfo($"[JDATA] - {FileName} does not exist. Creating one with default value...");
                data = defaultValue;
                Save();
                return false;
            }
        }
    }
}
