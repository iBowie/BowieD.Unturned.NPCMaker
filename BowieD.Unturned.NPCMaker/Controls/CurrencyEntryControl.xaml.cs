﻿using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Windows.Controls;

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
                img.Source = ThumbnailManager.CreateThumbnail(imagePath);
            }
        }

        public void UpdateFormat(string newFormat)
        {
            try
            {
                footer.Text = string.Format(newFormat, Entry.Value);
            }
            catch
            {
                footer.Text = Entry.Value.ToString();
            }
        }
    }
}
