using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BowieD.Unturned.NPCMaker.XAML
{
    public class LocalizeExtension : MarkupExtension
    {
        public string Key { get; set; }
        public Binding KeySource { get; set; }
        public LocalizeExtension() { }
        public LocalizeExtension(string key)
        {
            this.Key = key;
        }
        public LocalizeExtension(Binding keySource)
        {
            this.KeySource = keySource;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var providerValueTarget = serviceProvider as IProvideValueTarget;
            var multiBinding = new MultiBinding()
            {
                Converter = new LocalizationConverter(Key),
                NotifyOnSourceUpdated = true
            };
            multiBinding.Bindings.Add(new Binding
            {
                Source = LocalizationManager.Current,
                Path = new PropertyPath("CurrentCulture")
            });
            if (KeySource != null)
                multiBinding.Bindings.Add(KeySource);
            return multiBinding.ProvideValue(serviceProvider);
        }
    }
}
