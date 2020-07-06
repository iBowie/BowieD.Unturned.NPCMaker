using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message_Page.xaml
    /// </summary>
    public partial class Dialogue_Message_Page : UserControl
    {
        private static bool isMenuInit = false;
        public Dialogue_Message_Page(string text)
        {
            InitializeComponent();
            textField.Text = text;
            deleteButton.Opacity = Hidden_Opacity;
            if (!isMenuInit)
            {
                textField.ContextMenu.Items.Add(ContextHelper.CreatePastePauseButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePasteNewLineButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePastePlayerNameButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePasteNPCNameButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePasteItalicButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePasteBoldButton());
                textField.ContextMenu.Items.Add(ContextHelper.CreatePasteColorMenu());
                isMenuInit = true;
            }
        }

        public string Page { get; private set; }

        public double Hidden_Opacity = 0.5;
        public double Visible_Opacity = 1;
        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            Page = textField.Text;
        }

        private void DeleteButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(deleteButton.Opacity, Visible_Opacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                deleteButton.BeginAnimation(OpacityProperty, anim);
            }
            else
            {
                deleteButton.Opacity = Visible_Opacity;
            }
        }

        private void DeleteButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppConfig.Instance.animateControls)
            {
                DoubleAnimation anim = new DoubleAnimation(deleteButton.Opacity, Hidden_Opacity, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                deleteButton.BeginAnimation(OpacityProperty, anim);
            }
            else
            {
                deleteButton.Opacity = Hidden_Opacity;
            }
        }
    }
}
