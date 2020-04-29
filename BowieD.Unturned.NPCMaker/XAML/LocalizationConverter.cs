using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class LocalizationConverter : IMultiValueConverter
    {
        private readonly string _key;
        public LocalizationConverter(string key)
        {
            _key = key;
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string dictName = _key.Split('_')[0];
                Localization.TranslationDictionary dict = Localization.LocalizationManager.Current.GetDictionary(dictName);
                if (dict != null)
                    return dict.Translate(_key.Substring(dictName.Length + 1));
                else
                    return _key;
            }
            catch
            {
                return _key;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
