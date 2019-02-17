using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                notificationContentGrid.Children.Add(uie);
            }
            animationStoryboard = new Storyboard();
            var anim2 = new DoubleAnimation
            {
                BeginTime = TimeSpan.FromSeconds(6),
                Duration = TimeSpan.FromSeconds(1),
                From = 1,
                To = 0
            };
            Storyboard.SetTarget(anim2, this);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(OpacityProperty));
            animationStoryboard.Children = new TimelineCollection(){ anim2 };
            animationStoryboard.Completed += AnimationStoryboard_Completed;
            this.parent = parent;
            animationStoryboard.Begin(this);
        }

        private void AnimationStoryboard_Completed(object sender, EventArgs e)
        {
            parent.Children.Remove(this);
        }

        private Storyboard animationStoryboard;
        private StackPanel parent;

        public virtual void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AnimationStoryboard_Completed(sender, null);
        }
    }
}
