using Newtonsoft.Json;
using System.IO;

namespace BowieD.NPCMaker.Data
{
    public abstract class JsonData<T> : IData<T> where T : new()
    {
        public virtual string FileName { get; }
        public virtual void Save(T value)
        {
            var content = JsonConvert.SerializeObject(this);
            File.WriteAllText(FileName, content);
        }
        public virtual T Load()
        {
            if (File.Exists(FileName))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(File.ReadAllText(FileName));
                }
                catch
                {
                    T newValue = new T();
                    Save(newValue);
                    return newValue;
                }
            }
            else
            {
                T newValue = new T();
                Save(newValue);
                return newValue;
            }
        }
    }
}
