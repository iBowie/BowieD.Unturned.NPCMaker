using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Filtering;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Markup;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для AssetPicker_Window.xaml
    /// </summary>
    public partial class AssetPicker_Window : Window
    {
        public AssetPicker_Window(Type assetType, params AssetFilter[] filters)
        {
            markup = new RichText();

            this.assetType = assetType;

            InitializeComponent();

            if (!GameAssetManager.HasImportedVanilla)
            {
                filter_origin_unturned.IsChecked = false;
                filter_origin_unturned.IsEnabled = false;
            }

            if (!GameAssetManager.HasImportedWorkshop)
            {
                filter_origin_workshop.IsChecked = false;
                filter_origin_workshop.IsEnabled = false;
            }

            filter_name.TextChanged += (sender, e) =>
            {
                _searchTimer.Stop();
                _searchTimer.Start();
            };
            filter_name.PreviewKeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case System.Windows.Input.Key.Enter:
                        {
                            e.Handled = true;
                            _searchTimer.Stop();
                            _searchTimer_Tick(this, new EventArgs());
                        }
                        break;
                }
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

            assetFilters = filters ?? new AssetFilter[0];

            foreach (var af in assetFilters)
            {
                CheckBox cb = new CheckBox()
                {
                    Content = af.Name,
                    IsChecked = af.IsEnabled,
                    Margin = new Thickness(10)
                };

                cb.Checked += (sender, e) =>
                {
                    af.IsEnabled = true;
                    RefreshAssets();
                };
                cb.Unchecked += (sender, e) =>
                {
                    af.IsEnabled = false;
                    RefreshAssets();
                };

                filterGrid.Children.Add(cb);
            }

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
        private AssetFilter[] assetFilters;

        private DispatcherTimer _searchTimer;

        public GameAsset SelectedAsset { get; private set; }

        void RefreshAssets()
        {
            ClearList();

            var assets = GameAssetManager.GetAllAssets(assetType);
            foreach (var a in assets)
            {
                string searchText = filter_name.Text;
                string searchTextLower = searchText.ToLowerInvariant();

                if (string.IsNullOrEmpty(searchText) ||
                    a.name.ToLowerInvariant().Contains(searchTextLower) ||
                    a.id.ToString().Contains(searchText) ||
                    a.guid.ToString("N").Contains(searchTextLower))
                {
                    switch (a.origin)
                    {
                        case EGameAssetOrigin.Project when filter_origin_project.IsChecked == false:
                        case EGameAssetOrigin.Unturned when filter_origin_unturned.IsChecked == false:
                        case EGameAssetOrigin.Workshop when filter_origin_workshop.IsChecked == false:
                            continue;
                    }

                    bool flag = true;

                    if (assetFilters.Length > 0)
                    {
                        foreach (var af in assetFilters)
                        {
                            if (af.IsEnabled)
                            {
                                if (!af.ShouldDisplay(a))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (flag)
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

            if (asset is IHasIcon hasIcon)
            {
                Image icon = new Image()
                {
                    Source = ThumbnailManager.CreateThumbnail(hasIcon.ImagePath),
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(1)
                };

                g.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
                g.Children.Add(icon);

                Grid.SetColumn(icon, 0);
                Grid.SetColumn(lid, 1);
                Grid.SetColumn(l, 2);
            }
            else if (asset is IHasThumbnail hasThumbnail)
            {
                Image icon = new Image()
                {
                    Source = hasThumbnail.Thumbnail,
                    Width = 32,
                    Height = 32,
                    Margin = new Thickness(1)
                };

                g.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
                g.Children.Add(icon);

                Grid.SetColumn(icon, 0);
                Grid.SetColumn(lid, 1);
                Grid.SetColumn(l, 2);
            }
            else if (asset is IHasAnimatedThumbnail hasAnimatedThumbnail)
            {
                var thumbs = hasAnimatedThumbnail.Thumbnails.ToList();

                if (thumbs.Count > 0)
                {
                    Image icon = new Image()
                    {
                        Source = thumbs.First(),
                        Width = 32,
                        Height = 32,
                        Margin = new Thickness(1)
                    };

                    DispatcherTimer dt = new DispatcherTimer()
                    {
                        Interval = new TimeSpan(0, 0, 3)
                    };
                    int last = 0;
                    dt.Tick += (sender, e) =>
                    {
                        last++;
                        if (last >= thumbs.Count)
                            last = 0;

                        icon.Source = thumbs[last];
                    };
                    dt.Start();

                    g.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
                    g.Children.Add(icon);

                    Grid.SetColumn(icon, 0);
                    Grid.SetColumn(lid, 1);
                    Grid.SetColumn(l, 2);
                }
                else
                {
                    Grid.SetColumn(lid, 0);
                    Grid.SetColumn(l, 1);
                }
            }
            else
            {
                Grid.SetColumn(lid, 0);
                Grid.SetColumn(l, 1);
            }

            if (asset is IHasToolTip hasToolTip)
            {
                g.ToolTip = hasToolTip.ToolTipContent;
            }
            else if (asset is IHasTextToolTip hasTextToolTip)
            {
                var sp = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                void addLabel(string text)
                {
                    sp.Children.Add(new Label()
                    {
                        Content = new TextBlock()
                        {
                            Text = text,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            TextAlignment = TextAlignment.Left
                        },
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(5)
                    });
                }

                foreach (var line in hasTextToolTip.GetToolTipLines())
                {
                    addLabel(line);
                }

                g.ToolTip = sp;
            }

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
