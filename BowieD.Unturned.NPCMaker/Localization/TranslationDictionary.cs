using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Localization
{
    public sealed class TranslationDictionary : Dictionary<string, string>
    {
        public new string this[string key] => Translate(key);
        public string Translate(string key, params object[] args)
        {
            if (ContainsKey(key))
            {
                string line = base[key];
                if (line == null)
                {
                    App.Logger.Log($"Key '{key}' has null translation.", Logging.ELogLevel.WARNING);
                }

                if (args?.Length > 0)
                {
                    try
                    {
                        return string.Format(line, args);
                    }
                    catch (Exception ex)
                    {
                        App.Logger.LogException($"Unable to translate key '{key}' in {LocalizationManager.Current.Name}", Logging.ELogLevel.ERROR, ex);
                        return key;
                    }
                }
                else
                {
                    return line;
                }
            }
            else
            {
                App.Logger.Log($"Missing translation key '{key}' in {LocalizationManager.Current.Name}", Logging.ELogLevel.WARNING);
                return key;
            }
        }

        public static void AddMissingKeys(TranslationDictionary dictionary, TranslationDictionary from)
        {
            foreach (var kv in from)
            {
                if (!dictionary.ContainsKey(kv.Key))
                {
                    App.Logger.Log($"Missing translation key '{kv.Key}' in {LocalizationManager.Current.Name}", Logging.ELogLevel.WARNING);
                    dictionary.Add(kv.Key, kv.Value);
                }
            }
        }
    }
}
