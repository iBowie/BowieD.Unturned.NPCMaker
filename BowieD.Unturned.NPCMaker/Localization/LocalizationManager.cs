using BowieD.Unturned.NPCMaker.Logging;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Localization
{
    public static class LocalizationManager
    {
        public static Localization Current { get; private set; } = new Localization();
        public static bool IsLoaded { get; private set; } = false;
        public static IEnumerable<ELanguage> SupportedLanguages()
        {
            yield return ELanguage.English;
            yield return ELanguage.Russian;
        }
        public static void LoadLanguage(ELanguage language)
        {
            App.Logger.LogInfo($"[LOCALIZATION] - Loading {language} localization...");
            try
            {
                using (StreamReader sr = new StreamReader(Application.GetResourceStream(new Uri($"Resources/Localization/{language}.json", UriKind.Relative)).Stream))
                {
                    string text = sr.ReadToEnd();
                    Current = Newtonsoft.Json.JsonConvert.DeserializeObject<Localization>(text);
                }
                try
                {
                    var dict = new ResourceDictionary();
                    foreach (var k in Current.Character)
                    {
                        dict.Add($"CHARACTER_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Condition)
                    {
                        dict.Add($"CONDITION_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Dialogue)
                    {
                        dict.Add($"DIALOGUE_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.General)
                    {
                        dict.Add($"GENERAL_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Interface)
                    {
                        dict.Add($"INTERFACE_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Mistakes)
                    {
                        dict.Add($"MISTAKES_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Notification)
                    {
                        dict.Add($"NOTIFICATION_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Options)
                    {
                        dict.Add($"OPTIONS_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Quest)
                    {
                        dict.Add($"QUEST_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Reward)
                    {
                        dict.Add($"REWARD_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.Vendor)
                    {
                        dict.Add($"VENDOR_{k.Key}", k.Value);
                    }
                    foreach (var k in Current.VendorItem)
                    {
                        dict.Add($"VENDORITEM_{k.Key}", k.Value);
                    }
                    App.Current.Resources.MergedDictionaries.Add(dict);
                    IsLoaded = true;
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"[LOCALIZATION] - Could not convert JSON to XAML.", ex);
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"[LOCALIZATION] - Could not load {language} localization.", ex);
            }
        }
        public static string GetLanguageName(ELanguage language)
        {
            switch (language)
            {
                case ELanguage.English:
                    return "English (US)";
                case ELanguage.Russian:
                    return "Русский (Russian)";
                default:
                    return "?";
            }
        }
        public static ELanguage GetLanguageFromCultureInfo(CultureInfo culture)
        {
            switch (culture.Name)
            {
                case "ru-RU":
                    return ELanguage.Russian;
                default:
                    return ELanguage.English;
            }
        }
    }
}
