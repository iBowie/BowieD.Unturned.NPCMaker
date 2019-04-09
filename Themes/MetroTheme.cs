using System;
using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public class MetroTheme : ITheme
    {
        public override string Name { get; set; }
        public string DictionaryName { get; set; }
        public override byte R { get; set; }
        public override byte G { get; set; }
        public override byte B { get; set; }

        public override void Apply()
        {
            try
            {
                MainWindow.Instance.Theme_Clear();
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
                Config.Configuration.Properties.currentTheme = this;

            }
            catch { Logging.Logger.Log($"Can't apply {Name} theme"); }
        }
    }
}
