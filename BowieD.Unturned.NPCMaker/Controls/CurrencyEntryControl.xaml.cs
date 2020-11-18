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

            header.Text = entry.ItemGUID;
            footer.Text = entry.Value.ToString();
        }

        public CurrencyEntry Entry { get; set; }
    }
}
