using BowieD.Unturned.NPCMaker.Editors;
using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.BetterControls
{
    /// <summary>
    /// Логика взаимодействия для Condition_ItemList.xaml
    /// </summary>
    public partial class Universal_ItemList : UserControl
    {
        public Universal_ItemList(object input, ReturnType type, bool localizable)
        {
            InitializeComponent();
            this.Value = input;
            mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
            if (type == ReturnType.Dialogue || type == ReturnType.Quest || type == ReturnType.Vendor)
                mainLabel.ToolTip = Value is NPC.NPCDialogue ? (Value as NPC.NPCDialogue).comment : Value is NPC.NPCQuest ? (Value as NPC.NPCQuest).comment : Value is NPC.NPCVendor ? (Value as NPC.NPCVendor).comment : Value;
            else
                mainLabel.ToolTip = Value is IHasDisplayName displayName ? displayName : Value;
            this.Localizable = localizable;
            this.Type = type;
        }

        public object Value { get; private set; }
        public bool Localizable { get; private set; }
        public ReturnType Type { get; private set; }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Type == ReturnType.Condition)
            {
                NPC.Condition Condition = Value as NPC.Condition;
                BetterForms.Universal_ConditionEditor uce = new BetterForms.Universal_ConditionEditor(Condition, Localizable);
                if (uce.ShowDialog() == true)
                {
                    Value = uce.Result;
                    mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
                    mainLabel.ToolTip = Value;
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
                NPC.Reward reward = Value as NPC.Reward;
                BetterForms.Universal_RewardEditor ure = new BetterForms.Universal_RewardEditor(reward, Localizable);
                ure.ShowDialog();
                if (ure.DialogResult == true)
                {
                    Value = ure.Result;
                    mainLabel.Content = Value is IHasDisplayName ? (Value as IHasDisplayName).DisplayName : Value.ToString();
                    mainLabel.ToolTip = Value;
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
                BetterForms.Universal_VendorItemEditor uvie = new BetterForms.Universal_VendorItemEditor(Item);
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
                mainLabel.ToolTip = Value;
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
