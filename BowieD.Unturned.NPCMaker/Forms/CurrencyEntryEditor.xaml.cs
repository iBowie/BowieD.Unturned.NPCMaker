using BowieD.Unturned.NPCMaker.NPC.Currency;
using System.ComponentModel;
using System.Windows;

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
