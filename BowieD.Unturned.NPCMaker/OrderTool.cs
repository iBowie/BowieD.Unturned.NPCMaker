using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker
{
    public static class OrderTool
    {
        #region UI
        #region Generics
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
            Action animateAction;

            int index = container.IndexOf(element);
            container.Children.Remove(element);

            if (InputTool.IsKeyDown(Key.LeftShift)) // move to top
            {
                container.Children.Insert(0, element);

                animateAction = () =>
                {
                    int ampl = 0;
                    for (int i = 0; i < container.Children.Count - 2; i++)
                    {
                        AnimateGoDown(container.Children[i] as T);
                        ampl++;
                    }
                    AnimateGoUp(element, ampl);
                };
            }
            else if (InputTool.IsKeyDown(Key.LeftCtrl)) // move by 5
            {
                int newIndex = MathUtil.Clamp(index - 5, 0, container.Children.Count);

                container.Children.Insert(newIndex, element);

                animateAction = () =>
                {
                    int ampl = 0;
                    for (int i = newIndex; i <= index; i++)
                    {
                        AnimateGoDown(container.Children[i] as T);
                        ampl++;
                    }
                    AnimateGoUp(element, ampl);
                };
            }
            else
            {
                container.Children.Insert(index - 1, element);

                animateAction = () =>
                {
                    T upper = element;
                    T bottom = container.Children[index] as T;

                    AnimateSwap(upper, bottom);
                };
            }

            container.UpdateOrderButtons<T>();

            if (AppConfig.Instance.animateControls && animateAction != null)
            {
                animateAction.Invoke();
            }
        }
        public static void MoveDown<T>(this Panel container, T element) where T : UIElement, IHasOrderButtons
        {
            Action animateAction;

            int index = container.IndexOf(element);
            container.Children.Remove(element);

            if (InputTool.IsKeyDown(Key.LeftShift)) // move to bottom
            {
                container.Children.Add(element);

                animateAction = () =>
                {
                    int ampl = 0;
                    for (int i = 0; i < container.Children.Count - 2; i++)
                    {
                        AnimateGoUp(container.Children[i] as T);
                        ampl++;
                    }
                    AnimateGoDown(element, ampl);
                };
            }
            else if (InputTool.IsKeyDown(Key.LeftCtrl)) // move by 5
            {
                int newIndex = MathUtil.Clamp(index + 5, 0, container.Children.Count);

                container.Children.Insert(newIndex, element);

                animateAction = () =>
                {
                    int ampl = 0;
                    for (int i = index; i <= newIndex; i++)
                    {
                        AnimateGoUp(container.Children[i] as T);
                        ampl++;
                    }
                    AnimateGoDown(element, ampl);
                };
            }
            else
            {
                container.Children.Insert(index + 1, element);

                animateAction = () =>
                {
                    T upper = container.Children[index] as T;
                    T bottom = element;

                    AnimateSwap(upper, bottom);
                };
            }

            container.UpdateOrderButtons<T>();

            if (AppConfig.Instance.animateControls && animateAction != null)
            {
                animateAction.Invoke();
            }
        }
        public static void AnimateSwap<T>(T upper, T bottom) where T : UIElement, IHasOrderButtons
        {
            AnimateGoUp(upper);
            AnimateGoDown(bottom);
        }
        public static void AnimateGoUp<T>(T element, double amplitude = 1) where T : UIElement, IHasOrderButtons
        {
            DoubleAnimation upAnim = new DoubleAnimation(element.RenderSize.Height * amplitude, 0, new Duration(System.TimeSpan.FromSeconds(animationDuration)));
            element.Transform.BeginAnimation(TranslateTransform.YProperty, upAnim);
        }
        public static void AnimateGoDown<T>(T element, double amplitude = 1) where T : UIElement, IHasOrderButtons
        {
            DoubleAnimation downAnim = new DoubleAnimation(-element.RenderSize.Height * amplitude, 0, new Duration(System.TimeSpan.FromSeconds(animationDuration)));
            element.Transform.BeginAnimation(TranslateTransform.YProperty, downAnim);
        }
        const double animationDuration = 0.25;
        #endregion
        public static void UpdateOrderButtons(this Panel container)
        {
            foreach (var c in container.Children)
            {
                if (c is IHasOrderButtons ct)
                {
                    UpdateOrderButtons(container, ct);
                }
            }
        }
        public static void UpdateOrderButtons(this Panel container, IHasOrderButtons element, UIElement upButton, UIElement downButton)
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
        public static void UpdateOrderButtons(this Panel container, IHasOrderButtons element)
        {
            container.UpdateOrderButtons(element, element.UpButton, element.DownButton);
        }
        #endregion
    }
}
