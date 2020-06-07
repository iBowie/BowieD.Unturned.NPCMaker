using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BowieD.Unturned.NPCMaker
{
    public static class Util
    {
        public static ImageSource GetImageSource(this string value)
        {
            return value.StartsWith("pack://application") ? new BitmapImage(new Uri(value)) : new BitmapImage(new Uri("pack://application:,,,/" + value));
        }
        public static int IndexOf<T>(this Panel grid, T element) where T : UIElement
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] == element)
                    return i;
            }
            return -1;
        }
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
            {
                return null;
            }

            if (parentObject is T parent)
            {
                return parent;
            }
            return FindParent<T>(parentObject);
        }
        public static T FindChildren<T>(Grid parent) where T : UIElement
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
        public static T FindByName<T>(Window window, string name) where T : UIElement
        {
            object res = window.FindName(name);
            if (res is T result)
            {
                return result;
            }
            return null;
        }
        public static object FindByName(Window window, string name)
        {
            return window.FindName(name);
        }
        public static object FindByName(string name)
        {
            return MainWindow.Instance.FindName(name);
        }
        public static bool Contains(this ItemCollection collection, Func<object, bool> func)
        {
            foreach (object item in collection)
            {
                if (func.Invoke(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
