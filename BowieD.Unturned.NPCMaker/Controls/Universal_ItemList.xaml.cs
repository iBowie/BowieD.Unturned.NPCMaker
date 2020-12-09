using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Thumbnails;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Condition_ItemList.xaml
    /// </summary>
    public partial class Universal_ItemList : UserControl, IHasOrderButtons
    {
        public Universal_ItemList(object input, ReturnType type, bool showMoveButtons = false)
        {
            InitializeComponent();
            Value = input;
            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
            mainLabel.ToolTip = mainLabel.Content;
            Type = type;
            moveUpButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
            moveDownButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
            ShowMoveButtons = showMoveButtons;
        }
        public Universal_ItemList(object input, bool showMoveButtons = false)
        {
            InitializeComponent();
            Value = input;
            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
            mainLabel.ToolTip = mainLabel.Content;

            Type = AutoDetectType(input);

            moveUpButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
            moveDownButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
            ShowMoveButtons = showMoveButtons;
        }

        void updateIcon()
        {
            bool flag = true;
            switch (Value)
            {
                case RewardItem itemReward:
                    {
                        if (itemReward.ID > 0 && GameAssetManager.TryGetAsset<GameItemAsset>(itemReward.ID, out var asset))
                        {
                            icon.Source = ThumbnailManager.CreateThumbnail(asset.ImagePath);
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    break;
                case VendorItem vendorItem:
                    {
                        if (vendorItem.type == ItemType.ITEM)
                        {
                            if (vendorItem.id > 0 && GameAssetManager.TryGetAsset<GameItemAsset>(vendorItem.id, out var asset))
                            {
                                icon.Source = ThumbnailManager.CreateThumbnail(asset.ImagePath);
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                    break;
                default:
                    {
                        flag = false;
                    }
                    break;
            }

            if (flag)
                icon.Visibility = Visibility.Visible;
            else
                icon.Visibility = Visibility.Collapsed;
        }

        private object _value;
        public object Value 
        { 
            get => _value;
            private set
            {
                _value = value;

                updateIcon();
            }
        }
        public ReturnType Type { get; }
        public bool ShowMoveButtons { get; internal set; }

        public UIElement UpButton => moveUpButton;
        public UIElement DownButton => moveDownButton;
        public Transform Transform => animateTransform;

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            ClipboardManager.SetObject(Type, Value);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Type)
            {
                case ReturnType.Condition:
                    {
                        Condition Condition = Value as Condition;
                        Forms.Universal_ConditionEditor uce = new Forms.Universal_ConditionEditor(Condition);
                        uce.Owner = this.TryFindParent<Window>();
                        uce.ShowDialog();
                        if (uce.DialogResult == true)
                        {
                            Value = uce.Result;
                            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
                            mainLabel.ToolTip = mainLabel.Content;
                        }

                        break;
                    }
                case ReturnType.Dialogue:
                    return;
                case ReturnType.Quest:
                    return;
                case ReturnType.Reward:
                    {
                        Reward reward = Value as Reward;
                        Forms.Universal_RewardEditor ure = new Forms.Universal_RewardEditor(reward);
                        ure.Owner = this.TryFindParent<Window>();
                        ure.ShowDialog();
                        if (ure.DialogResult == true)
                        {
                            Value = ure.Result;
                            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
                            mainLabel.ToolTip = mainLabel.Content;
                        }

                        break;
                    }
                case ReturnType.Vendor:
                    return;
                case ReturnType.VendorItem:
                    {
                        VendorItem Item = Value as VendorItem;
                        bool old = Item.isBuy;
                        Forms.Universal_VendorItemEditor uvie = new Forms.Universal_VendorItemEditor(Item);
                        uvie.Owner = this.TryFindParent<Window>();
                        if (uvie.ShowDialog() == true)
                        {
                            VendorItem NewItem = uvie.Result as VendorItem;
                            if (old != NewItem.isBuy)
                            {
                                if (NewItem.isBuy)
                                {
                                    MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.RemoveItemSell(Util.FindParent<Universal_ItemList>(sender as Button));
                                    MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.AddItemBuy(NewItem);
                                }
                                else
                                {
                                    MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.RemoveItemBuy(Util.FindParent<Universal_ItemList>(sender as Button));
                                    MainWindow.Instance.MainWindowViewModel.VendorTabViewModel.AddItemSell(NewItem);
                                }
                            }
                            Value = NewItem;
                        }
                        mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
                        mainLabel.ToolTip = mainLabel.Content;
                        break;
                    }
                case ReturnType.Character:
                    return;
                case ReturnType.Object:
                    return;
            }
        }

        ReturnType AutoDetectType(object input)
        {
            switch (input)
            {
                case Reward _:
                    return ReturnType.Reward;
                case Condition _:
                    return ReturnType.Condition;
                case NPCDialogue _:
                    return ReturnType.Dialogue;
                case NPCVendor _:
                    return ReturnType.Vendor;
                case NPCQuest _:
                    return ReturnType.Quest;
                case VendorItem _:
                    return ReturnType.VendorItem;
                case NPCCharacter _:
                    return ReturnType.Character;
                default:
                    return ReturnType.Object;
            }
        }

        public enum ReturnType
        {
            Reward, Condition, Dialogue, Vendor, Quest, VendorItem, Object, Character,
            Currency
        }
    }
}
