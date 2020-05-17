using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message.xaml
    /// </summary>
    public partial class Dialogue_Message : UserControl, INotifyPropertyChanged
    {
        private ushort prev;

        public Dialogue_Message(NPC.NPCMessage message)
        {
            InitializeComponent();
            Message = message;
            DataContext = this;
        }

        public NPC.NPCMessage Message
        {
            get => new NPC.NPCMessage
            {
                pages = Pages,
                conditions = Conditions,
                rewards = Rewards,
                prev = Prev
            };
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
                Rewards = value.rewards;
                Prev = value.prev;

                PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(""));
            }
        }
        public Condition[] Conditions { get; set; }
        public Reward[] Rewards { get; set; }
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
        public ushort Prev
        {
            get 
            { 
                return prev; 
            }
            set
            {
                MessageBox.Show($"new prev: {value}");
                prev = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void AddPageButton_Click(object sender, RoutedEventArgs e)
        {
            Dialogue_Message_Page dmp = new Dialogue_Message_Page("");
            dmp.textField.TextChanged += TextField_TextChanged;
            dmp.deleteButton.Click += DeleteButton_Click;
            pagesGrid.Children.Add(dmp);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Dialogue_Message_Page parent = Util.FindParent<Dialogue_Message_Page>(sender as Button);
            pagesGrid.Children.Remove(parent);
        }

        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Conditions = ulv.Values.Cast<Condition>().ToArray();
        }
        private void EditRewardsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, false)).ToList(), Universal_ItemList.ReturnType.Reward);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Rewards = ulv.Values.Cast<Reward>().ToArray();
        }
    }
}
