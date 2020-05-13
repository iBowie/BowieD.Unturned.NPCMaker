using BowieD.Unturned.NPCMaker.Forms.MessageBoxes;
using System.Windows;

namespace BowieD.Unturned.NPCMaker
{
    public static class MessageBox
    {
        public static MessageBoxResult Show(string messageBoxText)
        {
            return Show(messageBoxText, string.Empty);
        }
        public static MessageBoxResult Show(string messageBoxText, string captionText)
        {
            return Show(messageBoxText, captionText, MessageBoxButton.OK);
        }
        public static MessageBoxResult Show(string messageBoxText, string captionText, MessageBoxButton messageBoxButton)
        {
            return Show(messageBoxText, captionText, MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult Show(string messageBoxText, string captionText, MessageBoxButton messageBoxButton, MessageBoxImage image)
        {
            try
            {
                MessageBox_Custom mbc = new MessageBox_Custom(messageBoxText, captionText, messageBoxButton, image);
                mbc.ShowDialog();
                return mbc.Result;
            }
            catch
            {
                System.Windows.MessageBoxButton orig;
                switch (messageBoxButton)
                {
                    case MessageBoxButton.OKCancel:
                        orig = System.Windows.MessageBoxButton.OKCancel;
                        break;
                    case MessageBoxButton.YesNo:
                        orig = System.Windows.MessageBoxButton.YesNo;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        orig = System.Windows.MessageBoxButton.YesNoCancel;
                        break;
                    case MessageBoxButton.None:
                    case MessageBoxButton.OK:
                    default:
                        orig = System.Windows.MessageBoxButton.OK;
                        break;
                }
                return System.Windows.MessageBox.Show(messageBoxText, captionText, orig, image);
            }
        }
    }
    public enum MessageBoxButton
    {
        None = 0b0000,

        OK = 0b0001,
        Yes = 0b0100,
        No = 0b1000,
        Cancel = 0b0010,

        OKCancel = OK | Cancel,
        YesNo = Yes | No,
        YesNoCancel = Yes | No | Cancel
    }
}
