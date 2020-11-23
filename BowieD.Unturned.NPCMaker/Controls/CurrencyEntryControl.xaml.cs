using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

                if (Guid.TryParse(value.ItemGUID, out var itemG))
                {
                    if (GameAssetManager.TryGetAsset<GameItemAsset>(itemG, out var asset))
                    {
                        headerText = asset.name;
                    }
                    else
                    {
                        headerText = value.ItemGUID;
                    }
                }
                else
                {
                    headerText = value.ItemGUID;
                }
                header.Text = headerText;
                footer.Text = value.Value.ToString();
            }
        }
    }
}
