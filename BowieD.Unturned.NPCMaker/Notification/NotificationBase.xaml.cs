using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Notification
{
    /// <summary>
    /// Логика взаимодействия для NotificationBase.xaml
    /// </summary>
    public partial class NotificationBase : UserControl
    {
        public NotificationBase(StackPanel parent, Brush background, params UIElement[] children)
        {
            InitializeComponent();
            mainBorder.Background = background;
            foreach (UIElement uie in children)
            {
                Label l = new Label
                {
                    Content = uie,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                notificationContentGrid.Children.Add(l);
            }
            animationStoryboard = new Storyboard();
            DoubleAnimation anim = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.5),
                From = AppConfig.Instance.animateControls ? 300 : 0,
                To = 0
            };
            DoubleAnimation endAnim = new DoubleAnimation()
            {
                BeginTime = TimeSpan.FromSeconds(5.5),
                Duration = TimeSpan.FromSeconds(1),
                From = 0,
                To = AppConfig.Instance.animateControls ? -60 : 0
            };
            anim.Completed += new EventHandler((sender, e) =>
            {
                tt.BeginAnimation(TranslateTransform.YProperty, endAnim);
            });
            tt.BeginAnimation(TranslateTransform.XProperty, anim);
            DoubleAnimation anim2 = new DoubleAnimation
            {
                BeginTime = TimeSpan.FromSeconds(6),
                Duration = TimeSpan.FromSeconds(1),
                From = 0.8,
                To = AppConfig.Instance.animateControls ? 0 : 0.8
            };
            Storyboard.SetTarget(anim2, this);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(OpacityProperty));
            animationStoryboard.Children = new TimelineCollection() { anim2 };
            animationStoryboard.Completed += AnimationStoryboard_Completed;
            this.parent = parent;
            animationStoryboard.Begin(this);
        }

        private void AnimationStoryboard_Completed(object sender, EventArgs e)
        {
            parent.Children.Remove(this);
        }

        private readonly Storyboard animationStoryboard;
        private readonly StackPanel parent;

        public virtual void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AnimationStoryboard_Completed(sender, null);
        }
    }
}
