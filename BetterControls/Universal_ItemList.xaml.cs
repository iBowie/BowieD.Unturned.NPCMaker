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
            mainTextBlock.Text = Value.ToString();
            mainTextBlock.ToolTip = Value.ToString();
            this.Localizable = localizable;
            this.Type = type;
        }

        //public static Universal_ItemList AutoDetect(object input, bool localizable)
        //{
        //    if (input is NPC.Condition cond)
        //        return new Universal_ItemList(cond, ReturnType.Condition, localizable);
        //    else if (input is NPC.Reward reward)
        //        return new Universal_ItemList(reward, ReturnType.Reward, localizable);
        //    else if (input is NPC.NPCDialogue dialogue)
        //        return new Universal_ItemList(dialogue, ReturnType.Dialogue, localizable);
        //    else if (input is NPC.NPCVendor vendor)
        //        return new Universal_ItemList(vendor, ReturnType.Vendor, localizable);
        //    else if (input is NPC.VendorItem item)
        //        return new Universal_ItemList(item, ReturnType.VendorItem, localizable);
        //    return null;
        //}

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
                    mainTextBlock.Text = Value.ToString();
                    mainTextBlock.ToolTip = Value.ToString();
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
                    mainTextBlock.Text = Value.ToString();
                    mainTextBlock.ToolTip = Value.ToString();
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
                            MainWindow.Instance.Vendor_Delete_Sell(Util.FindParent<Universal_ItemList>(sender as Button));
                            MainWindow.Instance.Vendor_Add_Buy(NewItem);
                        }
                        else
                        {
                            MainWindow.Instance.Vendor_Delete_Buy(Util.FindParent<Universal_ItemList>(sender as Button));
                            MainWindow.Instance.Vendor_Add_Sell(NewItem);
                        }
                    }
                    Value = NewItem;
                    mainTextBlock.Text = Value.ToString();
                    mainTextBlock.ToolTip = Value.ToString();
                }
            }
        }

        public enum ReturnType
        {
            Reward, Condition, Dialogue, Vendor, Quest, VendorItem
        }
    }
}
