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
        private bool allowNullImagePath = false;
        private bool allowNullName = false;
        private ulong fileID;

        private UGC_SelectorView(UGC ugc, string modPath)
        {
            InitializeComponent();

            this.modPath = modPath;
            this.fileID = ugc.FileID;

            allowNullImagePath = true;
            allowNullName = true;

            visibilityComboBox.SelectedIndex = ugc.Visibility;

            string keepAsBeen = LocalizationManager.Current.Interface["UGC_AsUploaded"];

            MahApps.Metro.Controls.TextBoxHelper.SetWatermark(nameTextBox, keepAsBeen);
            MahApps.Metro.Controls.TextBoxHelper.SetWatermark(descTextBox, keepAsBeen);

            CommonCtor();
        }
        private UGC_SelectorView(string modPath)
        {
            InitializeComponent();

            this.modPath = modPath;

            CommonCtor();
        }

        void CommonCtor()
        {
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
                if (!allowNullName && string.IsNullOrEmpty(nameTextBox.Text))
                    return false;

                if (allowNullImagePath)
                {
                    if (!string.IsNullOrEmpty(imagePath) && !File.Exists(imagePath))
                        return false;
                }
                else
                {
                    if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                        return false;
                }

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
                FileID = fileID,
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

        public static UGC_SelectorView SV_Create(string modPath)
        {
            UGC_SelectorView usv = new UGC_SelectorView(modPath);

            return usv;
        }
        public static UGC_SelectorView SV_Update(UGC currentUgc, string modPath)
        {
            UGC_SelectorView usv = new UGC_SelectorView(currentUgc, modPath);

            var bi = new BitmapImage();

            bi.BeginInit();

            bi.UriSource = new Uri(currentUgc.Preview);
            bi.DecodePixelHeight = 180;
            bi.CacheOption = BitmapCacheOption.OnLoad;

            bi.EndInit();

            usv.iconImage.Source = bi;

            return usv;
        }
    }
}
