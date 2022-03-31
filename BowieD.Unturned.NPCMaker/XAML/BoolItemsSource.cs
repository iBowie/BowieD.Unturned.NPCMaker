using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class BoolItemsSource : Collection<string>, IValueConverter
    {
        private static readonly bool[] BOOL_SOURCE = new bool[2]
        {
            false,
            true,
        };

        private IDictionary<bool, object> valueToNameMap;
        private IDictionary<object, bool> nameToValueMap;
        private bool _isInit = false;

        public string LocalizationPrefix { get; set; }
        public TranslationDictionary Dictionary { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!_isInit)
            {
                Initialize();
            }

            return valueToNameMap[(bool)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!_isInit)
            {
                Initialize();
            }

            return nameToValueMap[value];
        }

        private void Initialize()
        {
            valueToNameMap = BOOL_SOURCE
              .ToDictionary(b => b, GetDescription);
            nameToValueMap = valueToNameMap
              .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            Clear();

            foreach (string name in nameToValueMap.Keys)
            {
                Add(name);
            }

            _isInit = true;
        }

        private object GetDescription(bool value)
        {
            if (Dictionary is null)
            {
                return $"{LocalizationPrefix ?? string.Empty}{value}";
            }
            else
            {
                return Dictionary.Translate($"{LocalizationPrefix ?? string.Empty}{value}");
            }
        }
    }
}
