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
            MessageBox_Custom mbc = new MessageBox_Custom(messageBoxText, captionText, messageBoxButton, image);
            mbc.ShowDialog();
            return mbc.Result;
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
