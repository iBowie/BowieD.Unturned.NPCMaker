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
using System.Windows.Threading;

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
                _searchTimer.Stop();
                _searchTimer.Start();
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

            _searchTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            _searchTimer.Tick += _searchTimer_Tick;
        }

        private void _searchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            RefreshAssets();
        }

        private Type assetType;
        private IMarkup markup;

        private DispatcherTimer _searchTimer;

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
            g.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });

            TextBlock tb = new TextBlock();
            Label l = new Label()
            {
                Content = tb
            };
            TextBlock tbid = new TextBlock();
            Label lid = new Label()
            {
                Content = tbid
            };

            if (asset is GameDialogueAsset gda)
            {
                tb.Text = gda.dialogue.ContentPreview;
            }
            else
            {
                markup.Markup(tb, asset.name);
            }

            tbid.Text = asset.id.ToString();

            g.Children.Add(l);
            g.Children.Add(lid);

            Grid.SetColumn(lid, 0);
            Grid.SetColumn(l, 1);

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
