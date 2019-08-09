using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Notification
{
    public interface INotificationManager
    {
        void Notify(string text, double fontSize = 16, params Button[] buttons);
        void Clear();
    }
}
