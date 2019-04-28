using BowieD.Unturned.NPCMaker.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker
{
    [XmlInclude(typeof(MetroTheme))]
    public abstract class Theme
    {
        public abstract void Apply();
        public abstract byte R { get; set; }
        public abstract byte G { get; set; }
        public abstract byte B { get; set; }
        public abstract string Name { get; set; }
        public static void ClearThemes()
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