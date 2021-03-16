using BowieD.Unturned.NPCMaker.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message_Page.xaml
    /// </summary>
    public partial class Dialogue_Message_Page : DraggableUserControl, IHasOrderButtons
    {
        public Dialogue_Message_Page(string text)
        {
            InitializeComponent();
            textField.Text = text;

            if (Configuration.AppConfig.Instance.useOldStyleMoveUpDown)
            {
                dragRectGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                dragRect.MouseLeftButtonDown += DragControl_LMB_Down;
                dragRect.MouseLeftButtonUp += DragControl_LMB_Up;
                dragRect.MouseMove += DragControl_MouseMove;

                UpButton.Visibility = Visibility.Collapsed;
                DownButton.Visibility = Visibility.Collapsed;
            }

            textField.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Dialogue | ContextHelper.EContextOption.Group_TextEdit);
        }

        public string Page { get; private set; }

        public UIElement UpButton => moveUpButton;

        public UIElement DownButton => moveDownButton;

        public Transform Transform => tranform;

        public override TranslateTransform DragRenderTransform => tranform;
        public override FrameworkElement DragControl => dragRect;

        public double Hidden_Opacity = 0.5;
        public double Visible_Opacity = 1;
        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page = textField.Text;
        }
    }
}
