using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker
{
    public static class Util
    {
        public static bool IsEnglishChar(this char c) => "qwertyuiopasdfghjklzxcvbnm".Contains(c.ToString().ToLower());
        public static bool IsDigit(this char c) => char.IsDigit(c);
        public static ImageSource GetImageSource(this string value) => value.StartsWith("pack://application") ? new BitmapImage(new Uri(value)) : new BitmapImage(new Uri("pack://application:,,,/" + value));
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent)
            {
                return parent;
            }
            return FindParent<T>(parentObject);
        }
        public static T FindChildren<T>(System.Windows.Controls.Grid parent) where T : UIElement
        {
            try
            {
                foreach (UIElement ui in parent.Children)
                {
                    if (ui is T ii)
                    {
                        return ii;
                    }
                }
                return null;
            }
            catch { return null; }
        }
    }
}
