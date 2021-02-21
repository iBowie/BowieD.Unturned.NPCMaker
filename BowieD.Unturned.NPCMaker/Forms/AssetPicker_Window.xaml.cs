using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Filtering;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Markup;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
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

            _searchTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 500)
            };
            _searchTimer.Tick += _searchTimer_Tick;

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

            SetupTopBar();
            SetupAssets();
            RefreshAssets();
        }

        private void _searchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            RefreshAssets();
        }

        private Type assetType;
        private IMarkup markup, searchMarkup;
        private AssetFilter[] assetFilters;

        private DispatcherTimer _searchTimer;

        public GameAsset SelectedAsset { get; private set; }

        void RefreshAssets()
        {
            string searchText = filter_name.Text;
            string searchTextLower = searchText.ToLowerInvariant();

            searchMarkup = new SearchRichText(searchTextLower);

            foreach (Grid g in stack.Children)
            {
                if (g.Tag is GameAsset asset)
                {
                    string aName;
                    string vName;

                    if (asset is IHasNameOverride no)
                        vName = no.NameOverride;
                    else
                        vName = asset.name;

                    if (asset is ISearchNameOverride searchNameOverride)
                        aName = searchNameOverride.SearchNameOverride.ToLowerInvariant();
                    else if (asset is IHasNameOverride nameOverride)
                        aName = nameOverride.NameOverride.ToLowerInvariant();
                    else if (string.IsNullOrEmpty(asset.name))
                        aName = string.Empty;
                    else
                        aName = asset.name.ToLowerInvariant();

                    bool shouldDisplay;

                    if (string.IsNullOrEmpty(searchText) ||
                        aName.Contains(searchTextLower) ||
                        asset.id.ToString().Contains(searchText) ||
                        Guid.TryParse(searchTextLower, out var searchGuid) && asset.guid == searchGuid)
                    {
                        shouldDisplay = true;
                    }
                    else
                    {
                        shouldDisplay = false;
                    }

                    if (shouldDisplay)
                    {
                        switch (asset.origin)
                        {
                            case EGameAssetOrigin.Project when filter_origin_project.IsChecked == false:
                            case EGameAssetOrigin.Unturned when filter_origin_unturned.IsChecked == false:
                            case EGameAssetOrigin.Workshop when filter_origin_workshop.IsChecked == false:
                                shouldDisplay = false;
                                break;
                            default:
                                shouldDisplay = true;
                                break;
                        }
                    }

                    if (shouldDisplay && assetFilters.Length > 0)
                    {
                        foreach (var af in assetFilters)
                        {
                            if (af.IsEnabled)
                            {
                                if (!af.ShouldDisplay(asset))
                                {
                                    shouldDisplay = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (shouldDisplay)
                    {
                        g.Visibility = Visibility.Visible;

                        if (AppConfig.Instance.highlightSearch)
                        {
                            Label l = g.Children[0] as Label;
                            TextBlock tb = l.Content as TextBlock;

                            if (string.IsNullOrEmpty(searchText))
                            {
                                markup.Markup(tb, vName);
                            }
                            else
                            {
                                searchMarkup.Markup(tb, vName);
                            }
                        }
                    }
                    else
                        g.Visibility = Visibility.Collapsed;

                }
            }
        }
        void SetupAssets()
        {
            ClearList();

            IEnumerable<GameAsset> assets = GameAssetManager.GetAllAssets(assetType);

            IEnumerable<GameAsset> orderedAssets;

            switch (orderMode)
            {
                case EOrderByMode.ID_A:
                    orderedAssets = assets.OrderBy(d => d.origin).ThenBy(d => d.id);
                    break;
                case EOrderByMode.ID_D:
                    orderedAssets = assets.OrderBy(d => d.origin).ThenByDescending(d => d.id);
                    break;
                case EOrderByMode.Name_A:
                    orderedAssets = assets.OrderBy(d => d.origin).ThenBy(d =>
                    {
                        if (d is IHasNameOverride nameOverride)
                            return nameOverride.NameOverride;
                        else
                            return d.name;
                    });
                    break;
                case EOrderByMode.Name_D:
                    orderedAssets = assets.OrderBy(d => d.origin).ThenByDescending(d =>
                    {
                        if (d is IHasNameOverride nameOverride)
                            return nameOverride.NameOverride;
                        else
                            return d.name;
                    });
                    break;
                default:
                    orderedAssets = assets;
                    break;
            }

            if (typeof(ICreatable).IsAssignableFrom(assetType))
            {
                addEntryButton.Visibility = Visibility.Visible;

                addEntryButton.Command = new BaseCommand(() =>
                {
                    GameAsset createdA = (GameAsset)Activator.CreateInstance(assetType);

                    if (createdA is ICreatable createable)
                        createable.OnCreate();

                    Reorder();
                });
            }
            else
            {
                addEntryButton.Visibility = Visibility.Collapsed;
            }

            foreach (var asset in orderedAssets)
            {
                Grid g = new Grid()
                {
                    Margin = new Thickness(5),
                    Tag = asset
                };

                g.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
                g.ColumnDefinitions.Add(new ColumnDefinition());
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
                    Content = tb,
                    VerticalAlignment = VerticalAlignment.Center
                };
                TextBlock tbid = new TextBlock();
                Label lid = new Label()
                {
                    Content = tbid,
                    VerticalAlignment = VerticalAlignment.Center
                };

                if (asset is IHasNameOverride gda)
                {
                    markup.Markup(tb, gda.NameOverride);
                }
                else
                {
                    markup.Markup(tb, asset.name);
                }

                tbid.Text = asset.id.ToString();

                g.Children.Add(l);
                g.Children.Add(lid);

                Grid editableGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(5) };
                Grid deletableGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(5) };

                g.Children.Add(editableGrid);
                g.Children.Add(deletableGrid);

                if (asset is IEditable editable)
                {
                    Button editButton = new Button()
                    {
                        ToolTip = LocalizationManager.Current.Interface["AssetPicker_Editable_Edit_ToolTip"]
                    };

                    editButton.Content = new PackIconMaterial()
                    {
                        Kind = PackIconMaterialKind.Pencil,
                        Foreground = Application.Current.Resources["AccentColor"] as System.Windows.Media.Brush,
                        Width = 16,
                        Height = 16
                    };

                    editButton.Command = new BaseCommand(() =>
                    {
                        editable.Edit(this);

                        if (asset is IHasNameOverride gda1)
                        {
                            markup.Markup(tb, gda1.NameOverride);
                        }
                        else
                        {
                            markup.Markup(tb, asset.name);
                        }

                        tbid.Text = asset.id.ToString();
                    });

                    editableGrid.Children.Add(editButton);
                }

                if (asset is IDeletable deletable)
                {
                    Button deleteButton = new Button()
                    {
                        ToolTip = LocalizationManager.Current.Interface["AssetPicker_Deletable_Delete_ToolTip"]
                    };

                    deleteButton.Content = new PackIconMaterial()
                    {
                        Kind = PackIconMaterialKind.TrashCan,
                        Foreground = Application.Current.Resources["AccentColor"] as System.Windows.Media.Brush,
                        Width = 16,
                        Height = 16
                    };

                    deleteButton.Command = new BaseCommand(() =>
                    {
                        deletable.OnDelete();

                        Reorder();
                    });

                    deletableGrid.Children.Add(deleteButton);
                }

                if (asset is IHasIcon hasIcon)
                {
                    Image icon = new Image()
                    {
                        Source = ThumbnailManager.CreateThumbnail(hasIcon.ImagePath),
                        Width = 32,
                        Height = 32,
                        Margin = new Thickness(1),
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    g.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
                    g.Children.Add(icon);

                    Grid.SetColumn(icon, 0);
                    Grid.SetColumn(lid, 1);
                    Grid.SetColumn(l, 2);
                    Grid.SetColumn(editableGrid, 3);
                    Grid.SetColumn(deletableGrid, 4);
                }
                else if (asset is IHasThumbnail hasThumbnail)
                {
                    Image icon = new Image()
                    {
                        Source = hasThumbnail.Thumbnail,
                        Width = 32,
                        Height = 32,
                        Margin = new Thickness(1),
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    g.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
                    g.Children.Add(icon);

                    Grid.SetColumn(icon, 0);
                    Grid.SetColumn(lid, 1);
                    Grid.SetColumn(l, 2);
                    Grid.SetColumn(editableGrid, 3);
                    Grid.SetColumn(deletableGrid, 4);
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
                            Margin = new Thickness(1),
                            VerticalAlignment = VerticalAlignment.Center
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
                        Grid.SetColumn(editableGrid, 3);
                        Grid.SetColumn(deletableGrid, 4);
                    }
                    else
                    {
                        Grid.SetColumn(lid, 0);
                        Grid.SetColumn(l, 1);
                        Grid.SetColumn(editableGrid, 2);
                        Grid.SetColumn(deletableGrid, 3);
                    }
                }
                else
                {
                    tpbarGrid.ColumnDefinitions[0].Width = new GridLength(0);
                    Grid.SetColumn(lid, 0);
                    Grid.SetColumn(l, 1);
                    Grid.SetColumn(editableGrid, 2);
                    Grid.SetColumn(deletableGrid, 3);
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
        void ClearList()
        {
            stack.Children.Clear();
        }

        void Reorder()
        {
            ClearList();
            SetupAssets();
            RefreshAssets();
        }

        EOrderByMode orderMode;
        void SetupTopBar()
        {
            orderMode = EOrderByMode.ID_A;

            ordByIDIcon.Visibility = Visibility.Visible;
            ordByIDIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ChevronUp;

            ordByNameIcon.Visibility = Visibility.Collapsed;

            ordByID.MouseLeftButtonDown += (sender, e) =>
            {
                ordByNameIcon.Visibility = Visibility.Collapsed;

                EOrderByMode nextMode;

                switch (orderMode)
                {
                    case EOrderByMode.ID_A:
                        nextMode = EOrderByMode.ID_D;
                        break;
                    case EOrderByMode.ID_D:
                        nextMode = EOrderByMode.Default;
                        break;
                    default:
                        nextMode = EOrderByMode.ID_A;
                        break;
                }

                switch (nextMode)
                {
                    case EOrderByMode.ID_A:
                        ordByIDIcon.Visibility = Visibility.Visible;
                        ordByIDIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ChevronUp;
                        break;
                    case EOrderByMode.ID_D:
                        ordByIDIcon.Visibility = Visibility.Visible;
                        ordByIDIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ChevronDown;
                        break;
                    default:
                        ordByIDIcon.Visibility = Visibility.Collapsed;
                        break;
                }

                orderMode = nextMode;

                Reorder();
            };
            ordByName.MouseLeftButtonDown += (sender, e) =>
            {
                ordByIDIcon.Visibility = Visibility.Collapsed;

                EOrderByMode nextMode;

                switch (orderMode)
                {
                    case EOrderByMode.Name_A:
                        nextMode = EOrderByMode.Name_D;
                        break;
                    case EOrderByMode.Name_D:
                        nextMode = EOrderByMode.Default;
                        break;
                    default:
                        nextMode = EOrderByMode.Name_A;
                        break;
                }

                switch (nextMode)
                {
                    case EOrderByMode.Name_A:
                        ordByNameIcon.Visibility = Visibility.Visible;
                        ordByNameIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ChevronUp;
                        break;
                    case EOrderByMode.Name_D:
                        ordByNameIcon.Visibility = Visibility.Visible;
                        ordByNameIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.ChevronDown;
                        break;
                    default:
                        ordByNameIcon.Visibility = Visibility.Collapsed;
                        break;
                }

                orderMode = nextMode;

                Reorder();
            };
        }

        public enum EOrderByMode
        {
            Default,
            ID_A,
            ID_D,
            Name_A,
            Name_D
        }
    }
}
