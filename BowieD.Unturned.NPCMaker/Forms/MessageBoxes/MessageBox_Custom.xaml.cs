using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Forms.MessageBoxes
{
    /// <summary>
    /// Interaction logic for MessageBox_Custom.xaml
    /// </summary>
    public partial class MessageBox_Custom : Window
    {
        public MessageBox_Custom(string mainText, string captionText, MessageBoxButton buttons, MessageBoxImage image)
        {
            InitializeComponent();

            tBlock.Text = mainText;
            Title = captionText;

            var scr = ScreenHelper.GetCurrentScreen(this);
            label.MaxWidth = scr.Size.Width * 0.75;
            label.MaxHeight = scr.Size.Height * 0.75;

            if (buttons.HasFlag(MessageBoxButton.OK))
                stackPanel.Children.Add(CreateOK());
            if (buttons.HasFlag(MessageBoxButton.Yes))
                stackPanel.Children.Add(CreateYes());
            if (buttons.HasFlag(MessageBoxButton.No))
                stackPanel.Children.Add(CreateNo());
            if (buttons.HasFlag(MessageBoxButton.Cancel))
                stackPanel.Children.Add(CreateCancel());

            if (image != MessageBoxImage.None)
            {
                MahApps.Metro.IconPacks.PackIconMaterial icon = new MahApps.Metro.IconPacks.PackIconMaterial()
                {
                    Foreground = App.Current.Resources["AccentColor"] as Brush,
                    Height = 64,
                    Width = 64,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(10)
                };

                switch (image)
                {
                    case MessageBoxImage.Error:
                        icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.CloseCircleOutline;
                        break;
                    case MessageBoxImage.Exclamation:
                        icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircleOutline;
                        break;
                    case MessageBoxImage.Information:
                        icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.InformationOutline;
                        break;
                    case MessageBoxImage.Question:
                        icon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.HelpCircleOutline;
                        break;
                }

                grid.Children.Add(icon);
            }
        }

        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        Button CreateBase()
        {
            return new Button()
            {
                Margin = new Thickness(10)
            };
        }
        Button CreateOK()
        {
            var b = CreateBase();
            b.Content = "OK";
            b.Click += Click_OK;
            return b;
        }
        Button CreateCancel()
        {
            var b = CreateBase();
            b.Content = "Cancel";
            b.Click += Click_Cancel;
            return b;
        }
        Button CreateYes()
        {
            var b = CreateBase();
            b.Content = "Yes";
            b.Click += Click_Yes;
            return b;
        }
        Button CreateNo()
        {
            var b = CreateBase();
            b.Content = "No";
            b.Click += Click_No;
            return b;
        }

        void Click_OK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Result = MessageBoxResult.OK;
            Close();
        }
        void Click_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Result = MessageBoxResult.Cancel;
            Close();
        }
        void Click_Yes(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Result = MessageBoxResult.Yes;
            Close();
        }
        void Click_No(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Result = MessageBoxResult.No;
            Close();
        }
    }
}
