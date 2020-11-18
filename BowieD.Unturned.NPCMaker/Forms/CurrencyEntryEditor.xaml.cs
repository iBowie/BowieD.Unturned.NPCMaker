using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для CurrencyEntryEditor.xaml
    /// </summary>
    public partial class CurrencyEntryEditor : Window, INotifyPropertyChanged
    {
        public CurrencyEntryEditor(CurrencyEntry entry)
        {
            InitializeComponent();

            DataContext = this;

            this.Entry = entry;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public CurrencyEntry Entry { get; set; }
        public string GUID 
        { 
            get => Entry.ItemGUID;
            set
            {
                Entry.ItemGUID = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GUID)));
            }
        }
        public uint Value
        {
            get => Entry.Value;
            set
            {
                Entry.Value = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
