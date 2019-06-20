using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Localization
{
    public static class LocUtil
    {
        public static IEnumerable<string> SupportedLanguages()
        {
            yield return "en-US";
            yield return "ru-RU";
        }
        public static IEnumerable<CultureInfo> SupportedCultures()
        {
            foreach (var k in SupportedLanguages())
            {
                yield return new CultureInfo(k);
            }
        }
        public static string LocalizeInterface(string key)
        {
            if (MainWindow.Instance == null)
            {
                if (_interfaceLang != null && _interfaceLang.ContainsKey(key))
                    return _interfaceLang[key];
                return key;
            }
            else
            {
                var file = MainWindow.Instance.TryFindResource(key) as string;
                if (file == null)
                    return key;
                else
                    return file.Replace(@"\n", Environment.NewLine);
            }
        }
        public static string LocalizeInterface(string key, params object[] args)
        {
            if (LocalizeInterface(key) == key)
                return key;
            return string.Format(LocalizeInterface(key), args);
        }
        public static string LocalizeCondition(string key)
        {
            if (_conditionsLang != null && _conditionsLang.ContainsKey(key))
                return _conditionsLang[key];
            return key;
        }
        public static string LocalizeCondition(string key, params object[] args)
        {
            if (LocalizeCondition(key) == key)
                return key;
            return string.Format(LocalizeCondition(key), args);
        }
        public static string LocalizeReward(string key)
        {
            if (_rewardsLang != null && _rewardsLang.ContainsKey(key))
                return _rewardsLang[key];
            return key;
        }
        public static string LocalizeReward(string key, params object[] args)
        {
            if (LocalizeReward(key) == key)
                return key;
            return string.Format(LocalizeReward(key), args);
        }
        public static string LocalizeMistake(string key)
        {
            if (_mistakesLang != null && _mistakesLang.ContainsKey(key))
                return _mistakesLang[key];
            return key;
        }
        public static string LocalizeMistake(string key, params object[] args)
        {
            if (LocalizeMistake(key) == key)
                return key;
            return string.Format(LocalizeMistake(key), args);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="langCode">ex: en-US</param>
        public static void LoadLanguage(string langCode)
        {
            if (_isInit)
                throw new Exception("Language already loaded!");
            // lang.interface.LANGCODE.xaml
            // lang.conditions.LANGCODE.json
            // lang.rewards.LANGCODE.json
            // lang.mistakes.LANGCODE.json
            try
            {
                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Localization/lang.interface.")
                                              select d).First();
                ResourceDictionary dict = new ResourceDictionary()
                {
                    Source = new Uri($"Localization/lang.interface.{langCode}.xaml", UriKind.Relative)
                };
                if (oldDict != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
                _interfaceLang = new Dictionary<string, string>();
                foreach (var k in dict.Keys)
                {
                    var value = dict[k];
                    _interfaceLang.Add(k.ToString(), value.ToString());
                }
            }
            catch (Exception) { Debug.WriteLine("Could not load interface lang."); }
            try
            {
                var sri = Application.GetResourceStream(new Uri($"Localization/lang.conditions.{langCode}.json", UriKind.Relative));
                using (StreamReader sr = new StreamReader(sri.Stream))
                {
                    string text = sr.ReadToEnd();
                    _conditionsLang = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                }
                sri.Stream.Close();
            }
            catch (Exception) { Debug.WriteLine("Could not load conditions lang."); }
            try
            {
                using (StreamReader sr = new StreamReader(Application.GetResourceStream(new Uri($"Localization/lang.rewards.{langCode}.json", UriKind.Relative)).Stream))
                {
                    string text = sr.ReadToEnd();
                    _rewardsLang = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                }
            }
            catch (Exception) { Debug.WriteLine("Could not load rewards lang."); }
            try
            {
                using (StreamReader sr = new StreamReader(Application.GetResourceStream(new Uri($"Localization/lang.mistakes.{langCode}.json", UriKind.Relative)).Stream))
                {
                    string text = sr.ReadToEnd();
                    _mistakesLang = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                }
            }
            catch (Exception) { Debug.WriteLine("Could not load mistakes lang."); }
            _isInit = true;
            IsLoaded = true;
        }
        private static bool _isInit = false;
        private static Dictionary<string, string> 
            _conditionsLang = null,
            _rewardsLang = null,
            _mistakesLang = null,
            _interfaceLang = null;

        public static bool IsLoaded { get; private set; } = false;
    }
}
