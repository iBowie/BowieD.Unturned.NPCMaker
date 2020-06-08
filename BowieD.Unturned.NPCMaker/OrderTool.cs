using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker
{
    public static class OrderTool
    {
        #region UI
        public static void UpdateOrderButtons<T>(this Panel container) where T : UIElement, IHasOrderButtons
        {
            foreach (var c in container.Children)
            {
                if (c is T ct)
                {
                    UpdateOrderButtons(container, ct);
                }
            }
        }
        public static void UpdateOrderButtons<T>(this Panel container, T element, UIElement upButton, UIElement downButton) where T : UIElement
        {
            int index = container.IndexOf(element);

            if (index >= 1)
            {
                upButton.IsEnabled = true;
            }
            else
            {
                upButton.IsEnabled = false;
            }

            if (index < container.Children.Count - 1)
            {
                downButton.IsEnabled = true;
            }
            else
            {
                downButton.IsEnabled = false;
            }
        }
        public static void UpdateOrderButtons<T>(this Panel container, T element) where T : UIElement, IHasOrderButtons
        {
            container.UpdateOrderButtons(element, element.UpButton, element.DownButton);
        }
        public static void MoveUp<T>(this Panel container, T element) where T : UIElement, IHasOrderButtons
        {
            int index = container.IndexOf(element);
            container.Children.Remove(element);
            container.Children.Insert(index - 1, element);
            container.UpdateOrderButtons<T>();
        }
        public static void MoveDown<T>(this Panel container, T element) where T : UIElement, IHasOrderButtons
        {
            int index = container.IndexOf(element);
            container.Children.Remove(element);
            container.Children.Insert(index + 1, element);
            container.UpdateOrderButtons<T>();
        }
        #endregion
        public static void MoveUp<T>(this IList<T> list, T element)
        {
            int index = list.IndexOf(element);
            list.RemoveAt(index);
            list.Insert(index - 1, element);
        }
        public static void MoveDown<T>(this IList<T> list, T element)
        {
            int index = list.IndexOf(element);
            list.RemoveAt(index);
            list.Insert(index + 1, element);
        }
    }
}
