using BowieD.Unturned.NPCMaker.Localization;
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
            b.Items.Add(bm2);
            return b;
        }
        internal static MenuItem CreatePasteColorButton(string color = "#FFFFFF")
        {
            BrushConverter brushConverter = new BrushConverter();
            Brush clr;
            switch (color)
            {
                #region UNTURNED
                case "common":
                    clr = brushConverter.ConvertFromString("#ffffff") as Brush;
                    break;
                case "uncommon":
                    clr = brushConverter.ConvertFromString("#1f871f") as Brush;
                    break;
                case "rare":
                    clr = brushConverter.ConvertFromString("#4b64fa") as Brush;
                    break;
                case "epic":
                    clr = brushConverter.ConvertFromString("#964bfa") as Brush;
                    break;
                case "legendary":
                    clr = brushConverter.ConvertFromString("#c832fa") as Brush;
                    break;
                case "mythical":
                    clr = brushConverter.ConvertFromString("#fa3219") as Brush;
                    break;
                #endregion
                #region UNITY
                case "black":
                    clr = brushConverter.ConvertFromString("#000000") as Brush;
                    break;
                case "blue":
                    clr = brushConverter.ConvertFromString("#0000FF") as Brush;
                    break;
                case "cyan":
                    clr = brushConverter.ConvertFromString("#00FFFF") as Brush;
                    break;
                case "gray":
                case "grey":
                    clr = brushConverter.ConvertFromString("#7F7F7F") as Brush;
                    break;
                case "magenta":
                    clr = brushConverter.ConvertFromString("#FF00FF") as Brush;
                    break;
                case "green":
                    clr = brushConverter.ConvertFromString("#00FF00") as Brush;
                    break;
                case "red":
                    clr = brushConverter.ConvertFromString("#FF0000") as Brush;
                    break;
                case "white":
                    clr = brushConverter.ConvertFromString("#FFFFFF") as Brush;
                    break;
                case "yellow":
                    clr = brushConverter.ConvertFromString("#FFEB04") as Brush;
                    break;
                #endregion
                default:
                    try
                    {
                        clr = brushConverter.ConvertFromString(color) as Brush;
                    }
                    catch { clr = null; }
                    break;
            }
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
    }
}
