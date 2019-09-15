using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;
using System.Windows.Controls;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Condition_ItemList.xaml
    /// </summary>
    public partial class Universal_ItemList : UserControl
    {
        public Universal_ItemList(object input, ReturnType type, bool localizable, bool showMoveButtons = false)
        {
            InitializeComponent();
            this.Value = input;
            mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
            mainLabel.ToolTip = mainLabel.Content;
            this.Localizable = localizable;
            this.Type = type;
            moveUpButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
            moveDownButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
        }

        public object Value { get; private set; }
        public bool Localizable { get; private set; }
        public ReturnType Type { get; private set; }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Type == ReturnType.Condition)
            {
                Condition Condition = Value as Condition;
                Forms.Universal_ConditionEditor uce = new Forms.Universal_ConditionEditor(Condition, Localizable);
                if (uce.ShowDialog() == true)
                {
                    Value = uce.Result;
                    mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
                    mainLabel.ToolTip = mainLabel.Content;
                }
            }
            else if (Type == ReturnType.Dialogue)
            {
                return;
            }
            else if (Type == ReturnType.Quest)
            {
                return;
            }
            else if (Type == ReturnType.Reward)
            {
                Reward reward = Value as Reward;
                Forms.Universal_RewardEditor ure = new Forms.Universal_RewardEditor(reward, Localizable);
                ure.ShowDialog();
                if (ure.DialogResult == true)
                {
                    Value = ure.Result;
                    mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
                    mainLabel.ToolTip = mainLabel.Content;
                }
            }
            else if (Type == ReturnType.Vendor)
            {
                return;
            }
            else if (Type == ReturnType.VendorItem)
            {
                NPC.VendorItem Item = Value as NPC.VendorItem;
                bool old = Item.isBuy;
                Forms.Universal_VendorItemEditor uvie = new Forms.Universal_VendorItemEditor(Item);
                if (uvie.ShowDialog() == true)
                {
                    NPC.VendorItem NewItem = uvie.Result as NPC.VendorItem;
                    if (old != NewItem.isBuy)
                    {
                        if (NewItem.isBuy)
                        {
                            (MainWindow.VendorEditor as VendorEditor).RemoveItemSell(Util.FindParent<Universal_ItemList>(sender as Button));
                            (MainWindow.VendorEditor as VendorEditor).AddItemBuy(NewItem);
                        }
                        else
                        {
                            (MainWindow.VendorEditor as VendorEditor).RemoveItemBuy(Util.FindParent<Universal_ItemList>(sender as Button));
                            (MainWindow.VendorEditor as VendorEditor).AddItemSell(NewItem);
                        }
                    }
                    Value = NewItem;
                }
                mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
                mainLabel.ToolTip = mainLabel.Content;
            }
            else if (Type == ReturnType.Character)
                return;
            else if (Type == ReturnType.Object)
                return;
        }

        public enum ReturnType
        {
            Reward, Condition, Dialogue, Vendor, Quest, VendorItem, Object, Character
        }
    }
}
