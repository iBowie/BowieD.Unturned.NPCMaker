using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Message.xaml
    /// </summary>
    public partial class Dialogue_Message : DraggableUserControl, INotifyPropertyChanged, IHasOrderButtons
    {
        public Dialogue_Message(NPC.NPCMessage message)
        {
            InitializeComponent();
            Message = message;

            ContextMenu pbmenu = new ContextMenu();
            pbmenu.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(GameDialogueAsset), (asset) =>
            {
                Prev = asset.id;
                prevBox.Value = asset.id;
            }, "Control_SelectAsset_Dialogue", MahApps.Metro.IconPacks.PackIconMaterialKind.Chat));

            prevBox.ContextMenu = pbmenu;

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

            DataContext = this;
        }

        public NPC.NPCMessage Message
        {
            get => new NPC.NPCMessage
            {
                pages = Pages,
                conditions = Conditions.ToList(),
                rewards = Rewards.ToList(),
                prev = Prev
            };
            set
            {
                foreach (string page in value.pages)
                {
                    AddPage(page);
                }
                Conditions = value.conditions.ToArray();
                Rewards = value.rewards.ToArray();
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
        public ushort Prev { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UIElement UpButton => moveUpButton;
        public UIElement DownButton => moveDownButton;
        public Transform Transform => animateTransform;

        public override TranslateTransform DragRenderTransform => animateTransform;
        public override FrameworkElement DragControl => dragRect;

        private void AddPageButton_Click(object sender, RoutedEventArgs e)
        {
            AddPage();
        }

        private void AddPage(string content = "")
        {
            Dialogue_Message_Page dmp = new Dialogue_Message_Page(content);
            dmp.textField.TextChanged += TextField_TextChanged;
            dmp.deleteButton.Click += DeleteButton_Click;
            dmp.moveUpButton.Click += (object sender2, RoutedEventArgs e2) =>
            {
                OrderTool.MoveUp<Dialogue_Message_Page>(pagesGrid, dmp);
            };
            dmp.moveDownButton.Click += (object sender2, RoutedEventArgs e2) =>
            {
                OrderTool.MoveDown<Dialogue_Message_Page>(pagesGrid, dmp);
            };
            pagesGrid.Children.Add(dmp);

            OrderTool.UpdateOrderButtons(pagesGrid);
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
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToList(), Universal_ItemList.ReturnType.Condition);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Conditions = ulv.Values.Cast<Condition>().ToArray();
        }
        private void EditRewardsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, true)).ToList(), Universal_ItemList.ReturnType.Reward);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Rewards = ulv.Values.Cast<Reward>().ToArray();
        }
    }
}
