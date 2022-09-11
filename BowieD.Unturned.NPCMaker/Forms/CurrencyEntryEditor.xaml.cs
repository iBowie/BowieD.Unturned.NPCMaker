using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для CurrencyEntryEditor.xaml
    /// </summary>
    public partial class CurrencyEntryEditor : MetroWindow, INotifyPropertyChanged
    {
        public CurrencyEntryEditor(CurrencyEntry entry)
        {
            InitializeComponent();

            DataContext = this;

            this.Entry = entry;

            var cmenu = new ContextMenu();
            cmenu.Items.Add(ContextHelper.CreateSelectItemButton((asset) =>
            {
                GUID = asset.guid.ToString("N");
                guidBox.Text = asset.guid.ToString("N");
            }));
            guidBox.ContextMenu = cmenu;

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
        public bool IsVisibleInVendorMenu
        {
            get => Entry.IsVisibleInVendorMenu;
            set
            {
                Entry.IsVisibleInVendorMenu = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisibleInVendorMenu)));
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
