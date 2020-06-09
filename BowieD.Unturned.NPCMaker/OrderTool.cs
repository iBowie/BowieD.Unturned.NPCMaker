using BowieD.Unturned.NPCMaker.Configuration;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            MoveUp(container, element, out _, out _);
        }
        public static void MoveUp<T>(this Panel container, T element, out T upper, out T bottom) where T : UIElement, IHasOrderButtons
        {
            int index = container.IndexOf(element);
            container.Children.Remove(element);
            container.Children.Insert(index - 1, element);
            container.UpdateOrderButtons<T>();

            upper = element;
            bottom = container.Children[index] as T;

            if (AppConfig.Instance.animateControls)
            {
                AnimateSwap(upper, bottom);
            }
        }
        public static void MoveDown<T>(this Panel container, T element) where T : UIElement, IHasOrderButtons
        {
            MoveDown(container, element, out _, out _);
        }
        public static void MoveDown<T>(this Panel container, T element, out T upper, out T bottom) where T : UIElement, IHasOrderButtons
        {
            int index = container.IndexOf(element);
            container.Children.Remove(element);
            container.Children.Insert(index + 1, element);
            container.UpdateOrderButtons<T>();

            upper = container.Children[index] as T;
            bottom = element;

            if (AppConfig.Instance.animateControls)
            {
                AnimateSwap(upper, bottom);
            }
        }
        public static void AnimateSwap<T>(T upper, T bottom) where T : UIElement, IHasOrderButtons
        {
            const double duration = 0.25;

            DoubleAnimation upAnim = new DoubleAnimation(upper.RenderSize.Height, 0, new Duration(System.TimeSpan.FromSeconds(duration)));
            DoubleAnimation downAnim = new DoubleAnimation(-bottom.RenderSize.Height, 0, new Duration(System.TimeSpan.FromSeconds(duration)));

            upper.Transform.BeginAnimation(TranslateTransform.YProperty, upAnim);
            bottom.Transform.BeginAnimation(TranslateTransform.YProperty, downAnim);
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
