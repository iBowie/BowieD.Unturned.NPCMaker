﻿using BowieD.Unturned.NPCMaker.NPC;
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
    public partial class Universal_ItemList : DraggableUserControl, IHasOrderButtons
    {
        public Universal_ItemList(object input, ReturnType type, bool showMoveButtons = false)
        {
            InitializeComponent();
            Value = input;
            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
            mainLabel.ToolTip = mainLabel.Content;
            Type = type;

            if (Configuration.AppConfig.Instance.useOldStyleMoveUpDown)
            {
                moveUpButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
                moveDownButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;

                newStyleMove.Visibility = Visibility.Collapsed;
            }
            else
            {
                dragRect.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;

                dragRect.MouseLeftButtonDown += DragControl_LMB_Down;
                dragRect.MouseLeftButtonUp += DragControl_LMB_Up;
                dragRect.MouseMove += DragControl_MouseMove;

                moveUpButton.Visibility = Visibility.Collapsed;
                moveDownButton.Visibility = Visibility.Collapsed;

                oldStyleMove.Visibility = Visibility.Collapsed;
            }

            ShowMoveButtons = showMoveButtons;
        }
        public Universal_ItemList(object input, bool showMoveButtons = false)
        {
            InitializeComponent();
            Value = input;
            mainLabel.Content = Value is IHasUIText ? (Value as IHasUIText).UIText : Value.ToString();
            mainLabel.ToolTip = mainLabel.Content;

            Type = AutoDetectType(input);

            if (Configuration.AppConfig.Instance.useOldStyleMoveUpDown)
            {
                moveUpButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;
                moveDownButton.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;

                newStyleMove.Visibility = Visibility.Collapsed;
            }
            else
            {
                dragRect.Visibility = showMoveButtons ? Visibility.Visible : Visibility.Collapsed;

                dragRect.MouseLeftButtonDown += DragControl_LMB_Down;
                dragRect.MouseLeftButtonUp += DragControl_LMB_Up;
                dragRect.MouseMove += DragControl_MouseMove;

                moveUpButton.Visibility = Visibility.Collapsed;
                moveDownButton.Visibility = Visibility.Collapsed;

                oldStyleMove.Visibility = Visibility.Collapsed;
            }

            ShowMoveButtons = showMoveButtons;
        }

        void updateIcon()
        {
            if (Value is IUIL_Icon uilIcon)
            {
                if (uilIcon.UpdateIcon(out var img))
                {
                    icon.Visibility = Visibility.Visible;
                    icon.Source = img;
                }
                else
                {
                    icon.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                icon.Visibility = Visibility.Collapsed;
            }
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

        public override TranslateTransform DragRenderTransform => animateTransform;
        public override FrameworkElement DragControl => dragRect;

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
