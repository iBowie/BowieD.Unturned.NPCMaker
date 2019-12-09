using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class LocalizationConverter : IMultiValueConverter
    {
        private string _key;
        public LocalizationConverter(string key)
        {
            this._key = key;
        }
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string dictName = _key.Split('_')[0];
                var dict = Localization.LocalizationManager.Current.GetDictionary(dictName);
                return dict.Translate(string.Join("_", _key.Split('_').Skip(1)));
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
