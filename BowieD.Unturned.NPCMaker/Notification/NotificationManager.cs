using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Notification
{
    public class NotificationManager : INotificationManager
    {
        public void Notify(string text, double fontSize = 16, params Button[] buttons)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                TextAlignment = System.Windows.TextAlignment.Center,
                TextWrapping = System.Windows.TextWrapping.Wrap
            };
            NotificationBase notificationBase = new NotificationBase(MainWindow.Instance.notificationsStackPanel, MainWindow.Instance.Background, new UIElement[] { textBlock }.Concat(buttons).ToArray());
            notificationBase.Opacity = 0.8;
            MainWindow.Instance.notificationsStackPanel.Children.Add(notificationBase);
        }
    }
}
