using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Controls
{
    public abstract class DraggableUserControl : UserControl
    {
        public delegate void StartedDrag();
        public delegate void StoppedDrag();

        public event StartedDrag OnStartedDrag;
        public event StoppedDrag OnStoppedDrag;

        private bool _isDragging = false;
        private Panel _parent;
        private System.Windows.Point _dragOffset;

        public abstract TranslateTransform DragRenderTransform { get; }
        public abstract FrameworkElement DragControl { get; }
        public virtual bool IsHorizontal
        {
            get
            {
                var parent = VisualTreeHelper.GetParent(this);

                if (parent is StackPanel sp)
                {
                    return sp.Orientation == Orientation.Horizontal;
                }

                return false;
            }
        }

        protected virtual void DragControl_LMB_Up(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            _isDragging = false;

            DragRenderTransform.X = 0;
            DragRenderTransform.Y = 0;

            DragControl.ReleaseMouseCapture();

            if (Configuration.AppConfig.Instance.animateControls)
            {
                DoubleAnimation da = new DoubleAnimation(Opacity, 1, TimeSpan.FromSeconds(0.5));

                this.BeginAnimation(OpacityProperty, da);
            }
            else
            {
                Opacity = 1;
            }

            OnStoppedDrag?.Invoke();
        }

        protected virtual void DragControl_LMB_Down(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            _parent = VisualTreeHelper.GetParent(this) as Panel;

            _dragOffset = DragControl.TranslatePoint(new System.Windows.Point(0, 0), this);

            _dragOffset.X += DragControl.ActualWidth / 2;
            _dragOffset.Y += DragControl.ActualHeight / 2;

            _isDragging = true;

            DragControl.CaptureMouse();

            if (Configuration.AppConfig.Instance.animateControls)
            {
                DoubleAnimation da = new DoubleAnimation(Opacity, 0.5, TimeSpan.FromSeconds(0.5));

                this.BeginAnimation(OpacityProperty, da);
            }
            else
            {
                Opacity = 0.5;
            }

            OnStartedDrag?.Invoke();
        }

        protected virtual void DragControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging)
                return;

            var pos = e.GetPosition(this);

            var offset = _dragOffset;

            if (IsHorizontal)
            {
                double deltaX = pos.X - offset.X;

                int index = _parent.Children.IndexOf(this);

                if (deltaX < 0 && index == 0)
                    return;

                if (deltaX > 0 && index == _parent.Children.Count - 1)
                    return;

                if (deltaX == 0)
                    return;

                double tw = System.Math.Abs(deltaX);
                int ch = deltaX > 0 ? 1 : -1;

                int ti = index + ch;
                int count = 0;
                double m = ch == 1 ? Margin.Right : Margin.Left;

                while (tw > 0)
                {
                    FrameworkElement child = _parent.Children[ti] as FrameworkElement;

                    m += ch == 1 ? child.Margin.Left : child.Margin.Right;

                    m += child.ActualWidth;

                    if (tw >= m)
                    {
                        count += ch;
                        tw -= m;

                        m = ch == 1 ? child.Margin.Right : child.Margin.Left;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count != 0)
                {
                    _parent.Children.Remove(this);

                    int newIndex = MathUtil.Clamp(index + count, 0, _parent.Children.Count);

                    _parent.Children.Insert(newIndex, this);

                    DragRenderTransform.X = tw;
                }
                else
                {
                    DragRenderTransform.X = deltaX;
                }
            }
            else
            {
                double deltaY = pos.Y - offset.Y;

                int index = _parent.Children.IndexOf(this);

                if (deltaY < 0 && index == 0)
                    return;

                if (deltaY > 0 && index == _parent.Children.Count - 1)
                    return;

                if (deltaY == 0)
                    return;

                double tw = System.Math.Abs(deltaY);
                int ch = deltaY > 0 ? 1 : -1;

                int ti = index + ch;
                int count = 0;
                double m = ch == 1 ? Margin.Bottom : Margin.Top;

                while (tw > 0)
                {
                    FrameworkElement child = _parent.Children[ti] as FrameworkElement;

                    m += ch == 1 ? child.Margin.Top : child.Margin.Bottom;

                    m += child.ActualHeight;

                    if (tw >= m)
                    {
                        count += ch;
                        tw -= m;

                        m = ch == 1 ? child.Margin.Bottom : child.Margin.Top;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count != 0)
                {
                    _parent.Children.Remove(this);

                    int newIndex = MathUtil.Clamp(index + count, 0, _parent.Children.Count);

                    _parent.Children.Insert(newIndex, this);

                    DragRenderTransform.Y = tw;
                }
                else
                {
                    DragRenderTransform.Y = deltaY;
                }
            }
        }
    }
}
