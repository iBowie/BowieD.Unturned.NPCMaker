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
    public class EnumItemsSource : Collection<String>, IValueConverter
    {

        Type type;
        IDictionary<Object, Object> valueToNameMap;
        IDictionary<Object, Object> nameToValueMap;

        public Type Type
        {
            get { return this.type; }
            set
            {
                if (!value.IsEnum)
                    throw new ArgumentException("Type is not an enum.", "value");
                this.type = value;
                Initialize();
            }
        }
        public string LocalizationPrefix { get; set; }
        public TranslationDictionary Dictionary { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return this.valueToNameMap[value];
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return this.nameToValueMap[value];
        }

        void Initialize()
        {
            if (Dictionary == null || LocalizationPrefix == null)
            {
                this.valueToNameMap = this.type
                  .GetFields(BindingFlags.Static | BindingFlags.Public)
                  .ToDictionary(fi => fi.GetValue(null), GetDescription);
                this.nameToValueMap = this.valueToNameMap
                  .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            }
            else
            {
                this.valueToNameMap = this.type
                    .GetFields(BindingFlags.Static | BindingFlags.Public)
                    .ToDictionary(fi => fi.GetValue(null), k => (object)Dictionary.Translate(LocalizationPrefix + k.Name));
                this.nameToValueMap = this.valueToNameMap
                    .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            }
            Clear();
            foreach (String name in this.nameToValueMap.Keys)
                Add(name);
        }

        static Object GetDescription(FieldInfo fieldInfo)
        {
            var descriptionAttribute =
              (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            string[] splitted = descriptionAttribute == null ? new string[0] : descriptionAttribute.Description.Split('.');
            return descriptionAttribute != null ? LocalizationManager.Current.GetDictionary(splitted[0]).Translate(splitted[1]) : fieldInfo.Name;
        }

    }
}
