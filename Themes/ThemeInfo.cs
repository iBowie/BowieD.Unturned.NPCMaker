using System;
using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public class MetroTheme : ITheme
    {
        public string Name { get; set; }
        public string DictionaryName { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public void Apply()
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
    }
}
