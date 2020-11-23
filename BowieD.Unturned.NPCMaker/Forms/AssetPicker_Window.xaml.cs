using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Markup;
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
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для AssetPicker_Window.xaml
    /// </summary>
    public partial class AssetPicker_Window : Window
    {
        public AssetPicker_Window(Type assetType)
        {
            markup = new RichText();

            this.assetType = assetType;

            InitializeComponent();

            filter_name.TextChanged += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_project.Checked += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_project.Unchecked += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_unturned.Checked += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_unturned.Unchecked += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_workshop.Checked += (sender, e) =>
            {
                RefreshAssets();
            };
            filter_origin_workshop.Unchecked += (sender, e) =>
            {
                RefreshAssets();
            };

            RefreshAssets();
        }

        private Type assetType;
        private IMarkup markup;
        public GameAsset SelectedAsset { get; private set; }

        void RefreshAssets()
        {
            ClearList();

            var assets = GameAssetManager.GetAllAssets(assetType);
            foreach (var a in assets)
            {
                if (string.IsNullOrEmpty(filter_name.Text) || a.name.ToLowerInvariant().Contains(filter_name.Text.ToLowerInvariant()))
                {
                    switch (a.origin)
                    {
                        case EGameAssetOrigin.Project when filter_origin_project.IsChecked == false:
                        case EGameAssetOrigin.Unturned when filter_origin_unturned.IsChecked == false:
                        case EGameAssetOrigin.Workshop when filter_origin_workshop.IsChecked == false:
                            continue;
                    }

                    AddAssetToList(a);
                }
            }
        }
        void ClearList()
        {
            stack.Children.Clear();
        }
        void AddAssetToList(GameAsset asset)
        {
            Grid g = new Grid()
            {
                Margin = new Thickness(5)
            };
            g.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });
            int cOffset = 0;

            if (asset is GameItemAsset gia)
            {
                g.ColumnDefinitions.Add(new ColumnDefinition());
                cOffset = 1;

                Image img = new Image
                {
                    Source = new BitmapImage(gia.ImagePath),
                    Width = 31,
                    Height = 31,
                    Margin = new Thickness(1)
                };
                g.Children.Add(img);
            }

            TextBlock tb = new TextBlock();
            Label l = new Label()
            {
                Content = tb
            };

            markup.Markup(tb, asset.name);

            g.Children.Add(l);

            Grid.SetColumn(l, cOffset);

            g.MouseDown += (sender, e) =>
            {
                DialogResult = true;
                SelectedAsset = asset;
                Close();
            };
            stack.Children.Add(g);
        }
    }
}
