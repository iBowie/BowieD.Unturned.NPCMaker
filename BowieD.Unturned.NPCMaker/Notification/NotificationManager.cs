using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BowieD.Unturned.NPCMaker.Notification
{
    public class NotificationManager : INotificationManager
    {
        public static StackPanel panel { get; private set; }
        public NotificationManager()
        {
            panel = new StackPanel()
            {
                Margin = new Thickness(10),
                Name = "notificationsStackPanel",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 300
            };
            panel.SetBinding(FrameworkElement.HeightProperty, new Binding("Height")
            {
                ElementName = "mainWindow"
            });
            MainWindow.Instance.notificationOverlay.Children.Add(panel);
        }
        public void Notify(string text, double fontSize = 16, params Button[] buttons)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
            NotificationBase notificationBase = new NotificationBase(panel, MainWindow.Instance.Background, new UIElement[] { textBlock }.Concat(buttons).ToArray());
            notificationBase.Opacity = 0.8;
            panel.Children.Add(notificationBase);
        }
        public void Clear()
        {
            panel.Children.Clear();
        }
    }
}
