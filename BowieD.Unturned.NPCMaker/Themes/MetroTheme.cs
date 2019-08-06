using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public class MetroTheme : Theme
    {
        /// <summary>
        /// Name of theme
        /// </summary>
        public override string Name { get; set; }
        /// <summary>
        /// Metro dictionary name
        /// </summary>
        public string DictionaryName { get; set; }
        /// <summary>
        /// Accent color (Red)
        /// </summary>
        public override byte R { get; set; }
        /// <summary>
        /// Accent color (Green)
        /// </summary>
        public override byte G { get; set; }
        /// <summary>
        /// Accent color (Blue)
        /// </summary>
        public override byte B { get; set; }

        public override void Apply()
        {
            try
            {
                CurrentTheme?.Remove();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") });
                var resourceDictionary = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{DictionaryName}.xaml") };
                if (resourceDictionary == null)
                {
                    Config.Configuration.DefaultTheme.Apply();
                    return;
                }
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                App.Current.Resources["AccentColor"] = new SolidColorBrush(Color.FromRgb(R, G, B));
                Logging.Logger.Log($"Accent Color Set to ({R};{G};{B})");
                Config.Configuration.Properties.currentTheme = this;
                CurrentTheme = this;

            }
            catch { Logging.Logger.Log($"Can't apply {Name} theme"); }
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
                    Application.Current.Resources.MergedDictionaries.Remove(metroControls);
                if (metroFonts != null)
                    Application.Current.Resources.MergedDictionaries.Remove(metroFonts);
                if (metroThemes?.Count() > 0)
                {
                    foreach (var dic in metroThemes)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(dic);
                    }
                }
            }
            catch { Logger.Log("Could not clear theme"); }
        }
    }
}
