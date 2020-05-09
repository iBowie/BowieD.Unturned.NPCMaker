using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
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
            yield return ELanguage.Spanish;
            yield return ELanguage.French;
        }
        public static void LoadLanguage(ELanguage language)
        {
            App.Logger.Log($"[LOCALIZATION] - Loading {language} localization...");
            try
            {
                using (StreamReader sr = new StreamReader(Application.GetResourceStream(new Uri($"Resources/Localization/{language}.json", UriKind.Relative)).Stream))
                {
                    string text = sr.ReadToEnd();
                    Current = Newtonsoft.Json.JsonConvert.DeserializeObject<Localization>(text);
                    IsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"[LOCALIZATION] - Could not load {language} localization.", ex: ex);
            }
        }
        public static string GetLanguageName(ELanguage language)
        {
            switch (language)
            {
                case ELanguage.English:
                    return "English (US)";
                case ELanguage.Spanish:
                    return "Español (Spanish)";
                case ELanguage.French:
                    return "Français (French)";
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
                case "es-ES":
                    return ELanguage.Spanish;
                case "fr-FR":
                    return ELanguage.French;
                default:
                    return ELanguage.English;
            }
        }
    }
}
