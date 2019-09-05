using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Localization
{
    public sealed class TranslationDictionary : Dictionary<string, string>
    {
        public new string this[string key]
        {
            get
            {
                return Translate(key);
            }
        }
        public string Translate(string key, params object[] args)
        {
            if (this.ContainsKey(key))
            {
                string line = base[key];
                if (args?.Length > 0)
                    return string.Format(line, args);
                else
                    return line;
            }
            else
                return key;
        }
    }
}
