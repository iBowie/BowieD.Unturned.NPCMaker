using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Localization;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message_Page.xaml
    /// </summary>
    public partial class Dialogue_Message_Page : UserControl
    {
        private static bool isMenuInit = false;
        public Dialogue_Message_Page(string text)
        {
            InitializeComponent();
            textField.Text = text;
            deleteButton.Opacity = Hidden_Opacity;
            if (!isMenuInit)
            {
                textField.ContextMenu.Items.Add(CreatePastePauseButton());
                textField.ContextMenu.Items.Add(CreatePasteNewLineButton());
                textField.ContextMenu.Items.Add(CreatePastePlayerNameButton());
                textField.ContextMenu.Items.Add(CreatePasteNPCNameButton());
                textField.ContextMenu.Items.Add(CreatePasteColorMenu());
                isMenuInit = true;
            }
        }

        public string Page { get; private set; }

        public double Hidden_Opacity = 0.5;
        public double Visible_Opacity = 1;

        private MenuItem CreatePastePauseButton()
        {
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PastePause"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var context = (sender as MenuItem).Parent as ContextMenu;
                var target = context.PlacementTarget as TextBox;
                var pos = target.SelectionStart;
                var l = target.SelectionLength;
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<pause>");
            });
            return b;
        }
        private MenuItem CreatePasteNewLineButton()
        {
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PasteNewLine"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var context = (sender as MenuItem).Parent as ContextMenu;
                var target = context.PlacementTarget as TextBox;
                var pos = target.SelectionStart;
                var l = target.SelectionLength;
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<br>");
            });
            return b;
        }
        private MenuItem CreatePastePlayerNameButton()
        {
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PastePlayerName"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var context = (sender as MenuItem).Parent as ContextMenu;
                var target = context.PlacementTarget as TextBox;
                var pos = target.SelectionStart;
                var l = target.SelectionLength;
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_char>");
            });
            return b;
        }
        private MenuItem CreatePasteNPCNameButton()
        {
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PasteNPCName"]
            };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var context = (sender as MenuItem).Parent as ContextMenu;
                var target = context.PlacementTarget as TextBox;
                var pos = target.SelectionStart;
                var l = target.SelectionLength;
                target.Text = target.Text.Remove(pos, l).Insert(pos, "<name_npc>");
            });
            return b;
        }
        private MenuItem CreatePasteColorMenu()
        {
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PasteColor"]
            };
            var bm1 = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PasteColor_Unturned"]
            };
            bm1.Items.Add(CreatePasteColorButton("common"));
            bm1.Items.Add(CreatePasteColorButton("uncommon"));
            bm1.Items.Add(CreatePasteColorButton("rare"));
            bm1.Items.Add(CreatePasteColorButton("epic"));
            bm1.Items.Add(CreatePasteColorButton("legendary"));
            bm1.Items.Add(CreatePasteColorButton("mythical"));
            b.Items.Add(bm1);
            var bm2 = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface["Main_Tab_Dialogue_Page_PasteColor_Unity"]
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
        private MenuItem CreatePasteColorButton(string color = "#FFFFFF")
        {
            var brushConverter = new BrushConverter();
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
            var b = new MenuItem()
            {
                Header = LocalizationManager.Current.Interface.ContainsKey($"Main_Tab_Dialogue_Page_PasteColor_{color}") ? LocalizationManager.Current.Interface[$"Main_Tab_Dialogue_Page_PasteColor_{color}"] : color
            };
            if (clr != null)
                b.Icon = new Rectangle() { Width = 8, Height = 8, Fill = clr };
            b.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var submenu = (sender as MenuItem).Parent as MenuItem;
                var menu = submenu.Parent as MenuItem;
                var context = (menu as MenuItem).Parent as ContextMenu;
                var target = context.PlacementTarget as TextBox;
                var pos = target.SelectionStart;
                var l = target.SelectionLength;
                target.Text = target.Text.Insert(pos + l, "</color>").Insert(pos, $"<color={color}>");
            });
            return b;
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page = textField.Text;
        }

        private void DeleteButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(deleteButton.Opacity, Visible_Opacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                deleteButton.BeginAnimation(OpacityProperty, anim);
            }
            else
            {
                deleteButton.Opacity = Visible_Opacity;
            }
        }

        private void DeleteButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(deleteButton.Opacity, Hidden_Opacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                deleteButton.BeginAnimation(OpacityProperty, anim);
            }
            else
            {
                deleteButton.Opacity = Hidden_Opacity;
            }
        }
    }
}
