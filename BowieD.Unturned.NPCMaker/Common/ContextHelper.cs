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
                Kind = PackIconMaterialKind.FormatPageBreak,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.Clock,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.Account,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.AccountAlert,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
            return b;
        }
        internal static MenuItem CreatePasteColorMenu()
        {
            MenuItem b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Context_Dialogue_PasteColor"]
            };
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
                Kind = PackIconMaterialKind.Gamepad,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
            b.Items.Add(bm1);
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
                Kind = PackIconMaterialKind.Unity,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
            b.Items.Add(bm2);
            b.Icon = new PackIconMaterial()
            {
                Kind = PackIconMaterialKind.BasketFill,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.FormatItalic,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.FormatBold,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.ContentCopy,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.ContentDuplicate,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
                Kind = PackIconMaterialKind.ContentPaste,
                Foreground = App.Current.Resources["AccentColor"] as Brush
            };
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
