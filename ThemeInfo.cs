using System.Windows;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker
{
    public class ThemeInfo
    {
        public string Name { get; set; }
        public string DictionaryName { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public void Setup()
        {
            MainWindow.Instance.Theme_Clear();
            MainWindow.Instance.Theme_SetupMetro();
            var resourceDictionary = new ResourceDictionary() { Source = new System.Uri($"pack://application:,,,/MahApps.Metro;component/Styles/Themes/{DictionaryName}.xaml") };
            if (resourceDictionary == null)
            {
                Config.Configuration.DefaultTheme.Setup();
                return;
            }
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            App.Current.Resources["AccentColor"] = new SolidColorBrush(Color.FromRgb(R, G, B));
            Config.Configuration.Properties.currentTheme = this;
        }
    }
}
