using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.GameIntegration.Filtering
{
    public abstract class AssetFilter
    {
        public AssetFilter(string dict, string translationKey)
        {
            var d = LocalizationManager.Current.GetDictionary(dict);

            this.Name = d[translationKey];
        }

        public string Name { get; }

        public bool IsEnabled { get; set; }
        public abstract bool ShouldDisplay(GameAsset asset);
    }
}
