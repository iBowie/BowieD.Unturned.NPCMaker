using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Templating;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Common
{
    internal static class ContextHelper
    {
        internal static ContextMenu CreateContextMenu(EContextOption option, RoutedEventHandler copy = null, RoutedEventHandler dupl = null, RoutedEventHandler paste = null, Type context = null, Action<object> addTemplateMethod = null)
        {
            ContextMenu cmenu = new ContextMenu();

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
                cmenu.Items.Add(CreateCopyButton(copy));

            if (option.HasFlag(EContextOption.DuplicateObject))
                cmenu.Items.Add(CreateDuplicateButton(dupl));

            if (option.HasFlag(EContextOption.PasteObject))
                cmenu.Items.Add(CreatePasteButton(paste));

            if (option.HasFlag(EContextOption.AddFromTemplate))
                cmenu.Items.Add(CreateAddFromTemplateButton(context, addTemplateMethod));

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

            AddFromTemplate = 1 << 10,

            Color_Unturned = 1 << 11
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
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<br>");
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
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<pause>");
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
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_char>");
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
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_npc>");
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
                target.Text = target.Text.Insert(pos + l, "</color>").Insert(pos, $"<color={color}>");
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
                target.Text = target.Text.Insert(pos + l, "</i>").Insert(pos, "<i>");
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
                target.Text = target.Text.Insert(pos + l, "</b>").Insert(pos, "<b>");
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

        internal static MenuItem CreateAddFromTemplateButton(Type context, Action<object> add)
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Control_AddFromTemplate"]
            };
            b.Click += new RoutedEventHandler((sender, e) =>
            {
                OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = $"{LocalizationManager.Current.General["Project_TemplateFilter"]}|*.npctemplate",
                    Multiselect = false
                };
                if (ofd.ShowDialog() == true)
                {
                    try
                    {
                        var template = TemplateManager.LoadTemplate(ofd.FileName);

                        if (template != null)
                        {
                            if (TemplateManager.IsCorrectContext(template, context))
                            {
                                TemplateManager.PrepareTemplate(template);

                                if (template.Inputs.Count > 0)
                                    TemplateManager.AskForInput(template);

                                var result = TemplateManager.ApplyTemplate(template);

                                add.Invoke(result);
                            }
                            else
                            {
                                App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Template_InvalidContext"));
                            }
                        }
                        else
                        {
                            App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Template_InvalidFile"));
                        }
                    }
                    catch (Exception ex)
                    {
                        App.NotificationManager.Notify(LocalizationManager.Current.Notification.Translate("Template_Error"));
                        App.Logger.LogException("Could not add item from template", ex: ex);
                    }
                }
            });
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.FolderPlusOutline
            };
            (b.Icon as PackIconMaterial).SetResourceReference(PackIconMaterial.ForegroundProperty, "AccentColor");
            return b;
        }
    }
}
