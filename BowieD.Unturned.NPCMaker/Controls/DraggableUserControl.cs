using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

                int count = (int)(deltaX / ActualWidth);

                if (count != 0)
                {
                    _parent.Children.Remove(this);

                    int newIndex = MathUtil.Clamp(index + count, 0, _parent.Children.Count);

                    _parent.Children.Insert(newIndex, this);

                    DragRenderTransform.X = deltaX % ActualWidth;
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

                int count = (int)(deltaY / ActualHeight);

                if (count != 0)
                {
                    _parent.Children.Remove(this);

                    int newIndex = MathUtil.Clamp(index + count, 0, _parent.Children.Count);

                    _parent.Children.Insert(newIndex, this);

                    DragRenderTransform.Y = deltaY % ActualHeight;
                }
                else
                {
                    DragRenderTransform.Y = deltaY;
                }
            }
        }
    }
}
