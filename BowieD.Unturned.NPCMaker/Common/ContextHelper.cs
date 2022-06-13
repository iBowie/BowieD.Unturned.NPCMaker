using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Filtering;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.IconPacks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Common
{
    internal static class ContextHelper
    {
        internal static ContextMenu CreateContextMenu(EContextOption option)
        {
            ContextMenu cmenu = new ContextMenu();

            if (option.HasFlag(EContextOption.CopyText))
                cmenu.Items.Add(CreateCopyTextButton());

            if (option.HasFlag(EContextOption.PasteText))
                cmenu.Items.Add(CreatePasteTextButton());

            if (option.HasFlag(EContextOption.CutText))
                cmenu.Items.Add(CreateCutTextButton());

            if (option.HasFlag(EContextOption.NewLine))
                cmenu.Items.Add(CreatePasteNewLineButton());

            if (option.HasFlag(EContextOption.Pause))
                cmenu.Items.Add(CreatePastePauseButton());

            if (option.HasFlag(EContextOption.PlayerName))
                cmenu.Items.Add(CreatePastePlayerNameButton());

            if (option.HasFlag(EContextOption.NPCName))
                cmenu.Items.Add(CreatePasteNPCNameButton());

            bool
                flag1 = option.HasFlag(EContextOption.Color_Unity),
                flag2 = option.HasFlag(EContextOption.Color_Unturned);

            if (flag1 || flag2)
                cmenu.Items.Add(CreatePasteColorMenu(flag2, flag1));

            if (option.HasFlag(EContextOption.Italic))
                cmenu.Items.Add(CreatePasteItalicButton());

            if (option.HasFlag(EContextOption.Bold))
                cmenu.Items.Add(CreatePasteBoldButton());

            if (option.HasFlag(EContextOption.CopyObject))
                throw new ArgumentException("CopyObject is not supported here");

            if (option.HasFlag(EContextOption.DuplicateObject))
                throw new ArgumentException("DuplicateObject is not supported here");

            if (option.HasFlag(EContextOption.PasteObject))
                throw new ArgumentException("PasteObject is not supported here");

            return cmenu;
        }

        internal enum EContextOption
        {
            NewLine = 1 << 0,
            Pause = 1 << 1,
            PlayerName = 1 << 2,
            NPCName = 1 << 3,
            Color_Unity = 1 << 4,
            Italic = 1 << 5,
            Bold = 1 << 6,

            Group_Color = Color_Unity | Color_Unturned,
            Group_Rich = Group_Color | Italic | Bold,
            Group_Dialogue = Group_Rich | NewLine | Pause | PlayerName | NPCName,

            CopyObject = 1 << 7,
            DuplicateObject = 1 << 8,
            PasteObject = 1 << 9,

            Group_CopyPasteObject = CopyObject | DuplicateObject | PasteObject,

            [Obsolete("Not used by anything", true)]
            AddFromTemplate = 1 << 10,

            Color_Unturned = 1 << 11,

            CopyText = 1 << 12,
            PasteText = 1 << 13,
            CutText = 1 << 14,

            Group_TextEdit = CopyText | PasteText | CutText,

            FindReplace = 1 << 15,
        }

        internal static MenuItem CreatePasteNewLineButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteNewLine"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Remove(pos, l).Insert(pos, "<br>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.FormatPageBreak
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePastePauseButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PastePause"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Remove(pos, l).Insert(pos, "<pause>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.Clock
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePastePlayerNameButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PastePlayerName"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_char>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.Account
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteNPCNameButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteNPCName"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_npc>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.AccountAlert
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteColorMenu(bool includeUnturned = true, bool includeUnity = true)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteColor"]
            };

            if (includeUnturned)
            {
                MenuItem bm1 = new MenuItem()
                {
                    Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteColor_Unturned"]
                };
                bm1.Items.Add(CreatePasteColorButton("common"));
                bm1.Items.Add(CreatePasteColorButton("uncommon"));
                bm1.Items.Add(CreatePasteColorButton("rare"));
                bm1.Items.Add(CreatePasteColorButton("epic"));
                bm1.Items.Add(CreatePasteColorButton("legendary"));
                bm1.Items.Add(CreatePasteColorButton("mythical"));
                bm1.Icon = new PackIconMaterial()
                {
                    Kind = PackIconMaterialKind.Gamepad
                };
                (bm1.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
                b.Items.Add(bm1);
            }

            if (includeUnity)
            {
                MenuItem bm2 = new MenuItem()
                {
                    Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteColor_Unity"]
                };
                bm2.Items.Add(CreatePasteColorButton("black"));
                bm2.Items.Add(CreatePasteColorButton("blue"));
                bm2.Items.Add(CreatePasteColorButton("cyan"));
                bm2.Items.Add(CreatePasteColorButton("gray"));
                bm2.Items.Add(CreatePasteColorButton("green"));
                bm2.Items.Add(CreatePasteColorButton("magenta"));
                bm2.Items.Add(CreatePasteColorButton("red"));
                bm2.Items.Add(CreatePasteColorButton("white"));
                bm2.Items.Add(CreatePasteColorButton("yellow"));
                bm2.Icon = new PackIconMaterial()
                {
                    Kind = PackIconMaterialKind.Unity
                };
                (bm2.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
                b.Items.Add(bm2);
            }

            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.BasketFill
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteColorButton(string color = "#FFFFFF")
        {
            Brush clr = Coloring.ColorConverter.ParseColor(color);
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface.ContainsKey($"Context_Dialogue_PasteColor_{color}") ? LocalizationManager.Current.Interface[$"Context_Dialogue_PasteColor_{color}"] : color
            };
            if (clr != null)
            {
                b.Icon = new Rectangle() { Width = 8, Height = 8, Fill = clr };
            }

            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                MenuItem submenu = (sender as MenuItem).Parent as MenuItem;
                MenuItem menu = submenu.Parent as MenuItem;
                ContextMenu context = (menu as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Insert(pos + l, "</color>").Insert(pos, $"<color={Coloring.ColorConverter.BrushToHEX(clr)}>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            return b;
        }
        internal static MenuItem CreatePasteItalicButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteItalic"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Insert(pos + l, "</i>").Insert(pos, "<i>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.FormatItalic
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteBoldButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteBold"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;
                try
                {
                    target.BeginChange();
                    target.Text = target.Text.Insert(pos + l, "</b>").Insert(pos, "<b>");
                }
                finally
                {
                    target.EndChange();
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.FormatBold
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }

        internal static MenuItem CreateCopyButton(RoutedEventHandler copy)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_Copy"]
            };
            b.Click += copy;
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentCopy
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreateDuplicateButton(RoutedEventHandler dupl)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_Duplicate"]
            };
            b.Click += dupl;
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentDuplicate
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteButton(RoutedEventHandler paste)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_Paste"]
            };
            b.Click += paste;
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentPaste
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }

        internal static MenuItem CreateCopyTextButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_CopyText"]
            };
            b.Click += (object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;

                Clipboard.SetText(target.Text.Substring(pos, l));
            };
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentCopy
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreatePasteTextButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_PasteText"]
            };
            b.Click += (object sender, RoutedEventArgs e) =>
            {
                if (Clipboard.ContainsText())
                {
                    ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                    TextBox target = context.PlacementTarget as TextBox;
                    int pos = target.SelectionStart;
                    int l = target.SelectionLength;

                    string ctext = Clipboard.GetText();

                    switch (l)
                    {
                        case 0:
                            target.Text = target.Text.Insert(pos, ctext);
                            break;
                        default:
                            target.Text = target.Text.Substring(0, pos) + ctext + target.Text.Substring(pos + l);
                            break;
                    }
                }
            };
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentPaste
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreateCutTextButton()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_CutText"]
            };
            b.Click += (object sender, RoutedEventArgs e) =>
            {
                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                TextBox target = context.PlacementTarget as TextBox;
                int pos = target.SelectionStart;
                int l = target.SelectionLength;

                if (l > 0)
                {
                    switch (l)
                    {
                        case 0:
                            break;
                        default:
                            Clipboard.SetText(target.Text.Substring(pos, l));
                            target.Text = target.Text.Substring(0, pos) + target.Text.Substring(pos + l);
                            break;
                    }
                }
            };
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.ContentCut
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }

        private static MenuItem createSelectAssetButton<T>(Action<T> action, string key, PackIconMaterialKind icon, params AssetFilter[] filters) where T : GameAsset
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface[key]
            };
            b.Click += (object sender, RoutedEventArgs e) =>
            {
                AssetPicker_Window apw = new AssetPicker_Window(typeof(T), filters);
                apw.Owner = MainWindow.Instance;
                if (apw.ShowDialog() == true && apw.SelectedAsset is T asat)
                {
                    action.Invoke(asat);
                }
            };
            b.Icon = new PackIconMaterial()
            {
                Kind = icon
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreateSelectAssetButton(Type assetType, Action<IAssetPickable> action, string key, PackIconMaterialKind icon, params AssetFilter[] filters)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface[key]
            };
            b.Click += (object sender, RoutedEventArgs e) =>
            {
                AssetPicker_Window apw = new AssetPicker_Window(assetType, filters);
                apw.Owner = MainWindow.Instance;
                if (apw.ShowDialog() == true)
                {
                    action.Invoke(apw.SelectedAsset);
                }
            };
            b.Icon = new PackIconMaterial()
            {
                Kind = icon
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreateGenericButton(ICommand command, string key, PackIconMaterialKind icon)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface[key]
            };
            b.Command = command;
            b.Icon = new PackIconMaterial()
            {
                Kind = icon
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
        internal static MenuItem CreateSelectItemButton(Type assetType, Action<IAssetPickable> action)
        {
            return CreateSelectAssetButton(assetType, action, "Control_SelectAsset_Item", PackIconMaterialKind.Archive);
        }
        internal static MenuItem CreateSelectItemButton(Action<GameItemAsset> action, params AssetFilter[] filters)
        {
            return createSelectAssetButton<GameItemAsset>(action, "Control_SelectAsset_Item", PackIconMaterialKind.Archive, filters);
        }
        internal static MenuItem CreateSelectHatButton(Action<GameItemHatAsset> action)
        {
            return createSelectAssetButton<GameItemHatAsset>(action, "Control_SelectAsset_Hat", PackIconMaterialKind.HatFedora);
        }
        internal static MenuItem CreateSelectGlassesButton(Action<GameItemGlassesAsset> action)
        {
            return createSelectAssetButton<GameItemGlassesAsset>(action, "Control_SelectAsset_Glasses", PackIconMaterialKind.Glasses);
        }
        internal static MenuItem CreateSelectBackpackButton(Action<GameItemBackpackAsset> action)
        {
            return createSelectAssetButton<GameItemBackpackAsset>(action, "Control_SelectAsset_Backpack", PackIconMaterialKind.BagCarryOn);
        }
        internal static MenuItem CreateSelectPantsButton(Action<GameItemPantsAsset> action)
        {
            return createSelectAssetButton<GameItemPantsAsset>(action, "Control_SelectAsset_Pants", PackIconMaterialKind.TshirtCrew);
        }
        internal static MenuItem CreateSelectShirtButton(Action<GameItemShirtAsset> action)
        {
            return createSelectAssetButton<GameItemShirtAsset>(action, "Control_SelectAsset_Shirt", PackIconMaterialKind.TshirtCrew);
        }
        internal static MenuItem CreateSelectMaskButton(Action<GameItemMaskAsset> action)
        {
            return createSelectAssetButton<GameItemMaskAsset>(action, "Control_SelectAsset_Mask", PackIconMaterialKind.DominoMask);
        }
        internal static MenuItem CreateSelectVestButton(Action<GameItemVestAsset> action)
        {
            return createSelectAssetButton<GameItemVestAsset>(action, "Control_SelectAsset_Vest", PackIconMaterialKind.TshirtCrew);
        }

        internal static MenuItem CreateFindUnusedIDButton(Action<ushort> action, EGameAssetCategory category)
        {
            return CreateGenericButton(new BaseCommand(() =>
            {
                if (GameAssetManager.TryFindUnusedID(category, out var result))
                {
                    action.Invoke(result);
                }
                else
                {
                    MessageBox.Show(LocalizationManager.Current.Interface["Control_FindUnusedID_Failed"]);
                }
            }), "Control_FindUnusedID", PackIconMaterialKind.Magnify);
        }

        internal static MenuItem CreateFindReplaceButton(FindReplace.FindReplaceFormat format)
        {
            return CreateGenericButton(new BaseCommand(() =>
            {
                FindReplaceDialog frd = new FindReplaceDialog(format);

                frd.ShowDialog();
            }), "Control_FindReplace", PackIconMaterialKind.FindReplace);
        }
    }
}
