using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.ViewModels;
using BowieD.Unturned.NPCMaker.Workshop;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для UGC_SelectorView.xaml
    /// </summary>
    public partial class UGC_SelectorView : Window
    {
        public UGC_SelectorView(string modPath)
        {
            InitializeComponent();

            this.modPath = modPath;

            selectIconButton.Command = new BaseCommand(() =>
            {
                const string formats = "*.png;*.jpg";

                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = $"{LocalizationManager.Current.General.Translate("UGC_ImageFilter", formats)}|{formats}",
                    Multiselect = false,
                    CheckFileExists = true
                };

                if (ofd.ShowDialog() == true)
                {
                    FileInfo fi = new FileInfo(ofd.FileName);

                    if (fi.Length > 1000000)
                    {
                        MessageBox.Show(LocalizationManager.Current.Interface["UGC_Preview_Select_TooLarge"]);
                        return;
                    }

                    BitmapImage bi = new BitmapImage();

                    bi.BeginInit();

                    bi.UriSource = new Uri(fi.FullName);
                    bi.DecodePixelHeight = 180;
                    bi.CacheOption = BitmapCacheOption.OnLoad;

                    bi.EndInit();

                    iconImage.Source = bi;

                    imagePath = fi.FullName;
                }
            });

            confirmButton.Command = new AdvancedCommand(() =>
            {
                FinalizedUGC = FinalizeUGC();
                DialogResult = true;
                Close();
            }, (param) =>
            {
                if (string.IsNullOrEmpty(nameTextBox.Text))
                    return false;

                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                    return false;

                if (!Directory.Exists(modPath))
                    return false;

                return true;
            });

            cancelButton.Command = new BaseCommand(() =>
            {
                DialogResult = false;
                Close();
            });
        }

        private readonly string modPath;
        private string imagePath;

        private UGC FinalizeUGC()
        {
            return new UGC()
            {
                Name = nameTextBox.Text,
                Description = descTextBox.Text,
                Path = modPath,
                Preview = imagePath,
                AllowedIPs = allowedIPsTextBox.Text,
                Change = changeTextBox.Text,
                Visibility = visibilityComboBox.SelectedIndex
            };
        }

        public UGC FinalizedUGC { get; private set; }
    }
}
