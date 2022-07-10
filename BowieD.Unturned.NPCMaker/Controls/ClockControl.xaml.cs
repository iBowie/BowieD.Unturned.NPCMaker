using System;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для ClockControl.xaml
    /// </summary>
    public partial class ClockControl : UserControl
    {
        public ClockControl()
        {
            InitializeComponent();

            mainGrid.DataContext = this;

            hourHand.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                if (_isDraggingMinute)
                    return;

                if (hourHand.CaptureMouse())
                {
                    _isDraggingHour = true;
                }
            };

            hourHand.PreviewMouseMove += (sender, e) =>
            {
                if (_isDraggingHour)
                {
                    var p = e.GetPosition(clockGrid);
                    var c = new Point(32, 32);

                    var newAngle = GetAngleFromPoint(p, c);

                    if (SnapToTicksEnabled)
                        newAngle = Math.Round(newAngle / 30.0) * 30.0;

                    // rotHour.Angle = newAngle;

                    var currentTime = DisplayTime;

                    double r1, r2;

                    if (IsPMOverAM)
                    {
                        r1 = 12;
                        r2 = 24;
                    }
                    else
                    {
                        r1 = 0;
                        r2 = 12;
                    }

                    double newHour = MathUtil.Clamp(MathUtil.Map(newAngle, 0, 360, r1, r2), r1, r2 - 1);
                    double newMinutes = newHour * 60 + currentTime.Minutes;

                    DisplayTime = TimeSpan.FromMinutes(newMinutes);

                    SetAngleOfHour((int)newHour);
                }
            };

            hourHand.PreviewMouseLeftButtonUp += (sender, e) =>
            {
                hourHand.ReleaseMouseCapture();

                _isDraggingHour = false;
            };

            minuteHand.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                if (_isDraggingHour)
                    return;

                if (minuteHand.CaptureMouse())
                {
                    _isDraggingMinute = true;
                }
            };

            minuteHand.PreviewMouseMove += (sender, e) =>
            {
                if (_isDraggingMinute)
                {
                    var p = e.GetPosition(clockGrid);
                    var c = new Point(32, 32);

                    var newAngle = GetAngleFromPoint(p, c);

                    if (SnapToTicksEnabled)
                        newAngle = Math.Round(newAngle / 6.0) * 6.0;

                    // rotMin.Angle = newAngle;

                    var currentTime = DisplayTime;

                    double v = MathUtil.Clamp(MathUtil.Map(newAngle, 0, 360, 0, 60), 0, 59);
                    double newMinutes = currentTime.Hours * 60 + v;

                    DisplayTime = TimeSpan.FromMinutes(newMinutes);

                    SetAngleOfMinutes((int)v);
                }
            };

            minuteHand.PreviewMouseLeftButtonUp += (sender, e) =>
            {
                minuteHand.ReleaseMouseCapture();

                _isDraggingMinute = false;
            };

            pmAmSwitch.PreviewMouseLeftButtonDown += (sender, e) =>
            {
                if (IsPMOverAM)
                {
                    DisplayTime = DisplayTime.Add(TimeSpan.FromHours(-12));
                }
                else
                {
                    DisplayTime = DisplayTime.Add(TimeSpan.FromHours(12));
                }
            };
        }

        private bool _isDraggingMinute = false;
        private bool _isDraggingHour = false;

        public TimeSpan DisplayTime
        {
            get { return (TimeSpan)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        public static readonly DependencyProperty DisplayTimeProperty =
            DependencyProperty.Register("DisplayTime", typeof(TimeSpan), typeof(ClockControl), new PropertyMetadata(TimeSpan.Zero, DisplayTimeChangedCallback));

        public bool SnapToTicksEnabled
        {
            get { return (bool)GetValue(SnapToTicksEnabledProperty); }
            set { SetValue(SnapToTicksEnabledProperty, value); }
        }

        public static readonly DependencyProperty SnapToTicksEnabledProperty =
            DependencyProperty.Register("SnapToTicksEnabled", typeof(bool), typeof(ClockControl), new PropertyMetadata(true));

        public bool IsPMOverAM
        {
            get { return (bool)GetValue(IsPMOverAMProperty); }
            set { SetValue(IsPMOverAMProperty, value); }
        }

        public static readonly DependencyProperty IsPMOverAMProperty =
            DependencyProperty.Register("IsPMOverAM", typeof(bool), typeof(ClockControl), new PropertyMetadata(false, IsPMOverAMChangedCallback));

        private static void IsPMOverAMChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ClockControl cc &&
                !cc._isDraggingHour &&
                !cc._isDraggingMinute &&
                e.NewValue is bool newB)
            {
                if (newB)
                {
                    cc.amDigitsHours.Visibility = Visibility.Collapsed;
                    cc.pmDigitsHours.Visibility = Visibility.Visible;
                    cc.pmAmSwitch.Text = "PM";
                }
                else
                {
                    cc.amDigitsHours.Visibility = Visibility.Visible;
                    cc.pmDigitsHours.Visibility = Visibility.Collapsed;
                    cc.pmAmSwitch.Text = "AM";
                }
            }
        }
        private static void DisplayTimeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ClockControl cc &&
                !cc._isDraggingMinute &&
                !cc._isDraggingHour &&
                e.NewValue is TimeSpan newSpan)
            {
                // cc.rotHour.Angle = newSpan.Hours % 12.0 / 12.0 * 360.0;
                cc.SetAngleOfHour(newSpan.Hours);

                // cc.rotMin.Angle = newSpan.Minutes / 60.0 * 360.0;
                cc.SetAngleOfMinutes(newSpan.Minutes);

                cc.IsPMOverAM = newSpan.Hours >= 12;
            }
        }

        private static double GetAngleFromPoint(Point point, Point center)
        {
            double dy = point.Y - center.Y;
            double dx = point.X - center.X;

            double theta = Math.Atan2(dy, dx);

            double angle = ((theta * 180 / Math.PI) + 360) % 360;

            return angle;
        }

        private void SetAngleOfHour(int hour)
        {
            rotHour.Angle = hour % 12.0 / 12.0 * 360.0;
        }

        private void SetAngleOfMinutes(int minutes)
        {
            rotMin.Angle = minutes / 60.0 * 360.0;
        }
    }
}
