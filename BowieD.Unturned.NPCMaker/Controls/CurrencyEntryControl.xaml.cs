using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для CurrencyEntryControl.xaml
    /// </summary>
    public partial class CurrencyEntryControl : UserControl
    {
        public CurrencyEntryControl(CurrencyEntry entry)
        {
            InitializeComponent();

            this.Entry = entry;
        }

        private CurrencyEntry _entry;
        public CurrencyEntry Entry
        {
            get => _entry;
            set
            {
                _entry = value;

                string headerText;
                Uri imagePath;

                if (Guid.TryParse(value.ItemGUID, out var itemG))
                {
                    if (GameAssetManager.TryGetAsset<GameItemAsset>(itemG, out var asset))
                    {
                        headerText = asset.name;
                        imagePath = asset.ImagePath;
                    }
                    else
                    {
                        headerText = value.ItemGUID;
                        imagePath = GameItemAsset.DefaultImagePath;
                    }
                }
                else
                {
                    headerText = value.ItemGUID;
                    imagePath = GameItemAsset.DefaultImagePath;
                }
                header.Text = headerText;
                footer.Text = value.Value.ToString();
                img.Source = new BitmapImage(imagePath);
            }
        }
    }
}
