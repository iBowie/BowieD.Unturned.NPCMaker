using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Themes
{
    public class MetroTheme : Theme
    {
        static readonly BrushConverter _converter = new BrushConverter();
        /// <summary>
        /// Name of theme
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// Metro dictionary name
        /// </summary>
        public string DictionaryName { get; set; }
        public override string AccentColor { get; set; }
        public override string ForegroundColor { get; set; }
        public override string BackgroundColor { get; set; }

        public override void Apply()
        {
            try
            {
                CurrentTheme?.Remove();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });
                ResourceDictionary resourceDictionary = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{DictionaryName}.xaml") };
                if (resourceDictionary == null)
                {
                    throw new NullReferenceException();
                }
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                App.Current.Resources["AccentColor"] = _converter.ConvertFromString(AccentColor);
                if (DictionaryName[0] == 'L')
                {
                    App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#000000");
                    App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#FFFFFF");
                }
                else if (DictionaryName[0] == 'D')
                {
                    App.Current.Resources["ForegroundColor"] = _converter.ConvertFromString("#FFFFFF");
                    App.Current.Resources["BackgroundColor"] = _converter.ConvertFromString("#252525");
                }
            }
            catch { App.Logger.Log($"Can't apply {Name} theme"); }
        }
        public override void Remove()
        {
            try
            {
                ResourceDictionary metroControls = (from d in Application.Current.Resources.MergedDictionaries
                                                    where d.Source != null && d.Source.OriginalString == "pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml"
                                                    select d).FirstOrDefault();
                ResourceDictionary metroFonts = (from d in Application.Current.Resources.MergedDictionaries
                                                 where d.Source != null && d.Source.OriginalString == "pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml"
                                                 select d).FirstOrDefault();
                List<ResourceDictionary> metroThemes = (from d in Application.Current.Resources.MergedDictionaries
                                                        where d.Source != null && d.Source.OriginalString.StartsWith("pack://application:,,,/MahApps.Metro;component/Styles/Themes/")
                                                        select d).ToList();
                if (metroControls != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(metroControls);
                }

                if (metroFonts != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(metroFonts);
                }

                if (metroThemes?.Count() > 0)
                {
                    foreach (ResourceDictionary dic in metroThemes)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(dic);
                    }
                }
                ThemeManager.CurrentTheme = null;
            }
            catch { App.Logger.Log("Could not clear theme"); }
        }
    }
}
