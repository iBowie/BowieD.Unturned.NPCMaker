using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class EnumItemsSource : Collection<string>, IValueConverter
    {
        private Type type;
        private IDictionary<object, object> valueToNameMap;
        private IDictionary<object, object> nameToValueMap;

        public Type Type
        {
            get => type;
            set
            {
                if (!value.IsEnum)
                {
                    throw new ArgumentException("Type is not an enum.", "value");
                }

                type = value;
                Initialize();
            }
        }
        public string LocalizationPrefix { get; set; }
        public TranslationDictionary Dictionary { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return valueToNameMap[value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return nameToValueMap[value];
        }

        private void Initialize()
        {
            if (Dictionary == null || LocalizationPrefix == null)
            {
                valueToNameMap = type
                  .GetFields(BindingFlags.Static | BindingFlags.Public)
                  .Where(fi =>
                  {
                      var skillLock = fi.GetCustomAttribute<Configuration.SkillLockAttribute>();

                      if (skillLock is null)
                          return true;

                      if (Configuration.AppConfig.Instance.skillLevel >= skillLock.Level)
                          return true;

                      return false;
                  })
                  .ToDictionary(fi => fi.GetValue(null), GetDescription);
                nameToValueMap = valueToNameMap
                  .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            }
            else
            {
                valueToNameMap = type
                    .GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Where(fi =>
                    {
                        var skillLock = fi.GetCustomAttribute<Configuration.SkillLockAttribute>();

                        if (skillLock is null)
                            return true;

                        if (Configuration.AppConfig.Instance.skillLevel >= skillLock.Level)
                            return true;

                        return false;
                    })
                    .ToDictionary(fi => fi.GetValue(null), k => (object)Dictionary.Translate(LocalizationPrefix + k.Name));
                nameToValueMap = valueToNameMap
                    .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            }
            Clear();
            foreach (string name in nameToValueMap.Keys)
            {
                Add(name);
            }
        }

        private static object GetDescription(FieldInfo fieldInfo)
        {
            DescriptionAttribute descriptionAttribute =
              (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            string[] splitted = descriptionAttribute == null ? new string[0] : descriptionAttribute.Description.Split('.');
            return descriptionAttribute != null ? LocalizationManager.Current.GetDictionary(splitted[0]).Translate(splitted[1]) : fieldInfo.Name;
        }

    }
}
