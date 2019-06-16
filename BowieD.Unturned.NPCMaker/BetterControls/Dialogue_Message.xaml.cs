using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.BetterControls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message.xaml
    /// </summary>
    public partial class Dialogue_Message : UserControl
    {
        public Dialogue_Message(NPC.NPCMessage message)
        {
            InitializeComponent();
            this.Message = message;
        }
        
        public NPC.NPCMessage Message
        {
            get
            {
                var m = new NPC.NPCMessage();
                m.pages = Pages;
                m.conditions = Conditions;
                return m;
            }
            set
            {
                foreach (string page in value.pages)
                {
                    Dialogue_Message_Page dmp = new Dialogue_Message_Page(page);
                    dmp.textField.TextChanged += TextField_TextChanged;
                    dmp.deleteButton.Click += DeleteButton_Click;
                    pagesGrid.Children.Add(dmp);
                }
                Conditions = value.conditions;
            }
        }
        public Condition[] Conditions { get; set; }
        public List<string> Pages
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (UIElement ui in pagesGrid.Children)
                {
                    if (ui is Dialogue_Message_Page dmp)
                    {
                        ret.Add(dmp.Page);
                    }
                }
                return ret;
            }
        }

        private void AddPageButton_Click(object sender, RoutedEventArgs e)
        {
            Dialogue_Message_Page dmp = new Dialogue_Message_Page("");
            dmp.textField.TextChanged += TextField_TextChanged;
            dmp.deleteButton.Click += DeleteButton_Click;
            pagesGrid.Children.Add(dmp);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Util.FindParent<Dialogue_Message_Page>(sender as Button);
            pagesGrid.Children.Remove(parent);
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(Conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            Conditions = ulv.Values.Cast<Condition>().ToArray();
        }
    }
}
