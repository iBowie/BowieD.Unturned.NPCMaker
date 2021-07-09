using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_ListView.xaml
    /// </summary>
    public partial class Universal_ListView : Window
    {
        public Universal_ListView(LimitedList<Controls.Universal_ItemList> listUil, Controls.Universal_ItemList.ReturnType returnType)
        {
            InitializeComponent();
            addButton.Content = LocalizationManager.Current.Interface[$"ListView_{returnType}_Add"];
            Title = LocalizationManager.Current.Interface[$"ListView_{returnType}_Title"];
            ReturnType = returnType;
            Values = new LimitedList<object>(listUil.MaxItems);
            foreach (Controls.Universal_ItemList uil in listUil)
            {
                Add(uil);
            }
            double scale = AppConfig.Instance.scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            Height *= scale;
            Width *= scale;
            MinWidth *= scale;

            ContextMenu cmenu = new ContextMenu();

            cmenu.Items.Add(ContextHelper.CreateAddFromTemplateButton(ClipboardManager.GetTypeFromFormat(ClipboardManager.GetFormat(ReturnType)), (result) =>
            {
                Add(new Universal_ItemList(result));
            }));

            addButton.ContextMenu = cmenu;

            addButton.Command = new AdvancedCommand(() =>
            {
                switch (ReturnType)
                {
                    case Controls.Universal_ItemList.ReturnType.Condition:
                        Universal_ConditionEditor uce = new Universal_ConditionEditor();
                        uce.Owner = this;
                        if (uce.ShowDialog() == true)
                        {
                            Universal_ItemList a = new Controls.Universal_ItemList(uce.Result, Controls.Universal_ItemList.ReturnType.Condition, true);
                            Add(a);
                        }
                        break;
                    case Controls.Universal_ItemList.ReturnType.Dialogue:
                        SelectedValue = new NPC.NPCDialogue();
                        DialogResult = true;
                        Close();
                        break;
                    case Controls.Universal_ItemList.ReturnType.Vendor:
                        SelectedValue = new NPC.NPCVendor();
                        DialogResult = true;
                        Close();
                        break;
                    case Controls.Universal_ItemList.ReturnType.Reward:
                        Universal_RewardEditor ure = new Universal_RewardEditor();
                        ure.Owner = this;
                        if (ure.ShowDialog() == true)
                        {
                            Universal_ItemList aa = new Controls.Universal_ItemList(ure.Result, Controls.Universal_ItemList.ReturnType.Reward, true);
                            Add(aa);
                        }
                        break;
                    case Controls.Universal_ItemList.ReturnType.Quest:
                        SelectedValue = new NPC.NPCQuest();
                        DialogResult = true;
                        Close();
                        break;
                    case Universal_ItemList.ReturnType.Character:
                        SelectedValue = new NPC.NPCCharacter();
                        DialogResult = true;
                        Close();
                        break;
                }
            }, (p) =>
            {
                return Values.CanAdd;
            });
        }

        public bool ShowMoveButtons
        {
            get
            {
                switch (ReturnType)
                {
                    case Universal_ItemList.ReturnType.Condition:
                    case Universal_ItemList.ReturnType.VendorItem:
                    case Universal_ItemList.ReturnType.Reward:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement ui in mainGrid.Children)
            {
                Universal_ItemList ll = ui as Controls.Universal_ItemList;
                if (ll.Equals(Util.FindParent<Controls.Universal_ItemList>(sender as UIElement)))
                {
                    mainGrid.Children.Remove(ll);
                    break;
                }
            }
            LimitedList<object> newValues = new LimitedList<object>(Values.MaxItems);
            foreach (UIElement ui in mainGrid.Children)
            {
                if (ui is Controls.Universal_ItemList uil)
                {
                    newValues.Add(uil.Value);
                }
            }
            Values = newValues;

            mainGrid.UpdateOrderButtons<Universal_ItemList>();
        }

        public LimitedList<object> Values { get; private set; }
        public Controls.Universal_ItemList.ReturnType ReturnType { get; private set; }

        public object SelectedValue { get; private set; }

        public void UpdateValues()
        {
            Values.Clear();
            foreach (UIElement uie in mainGrid.Children)
            {
                if (uie is Universal_ItemList uil)
                {
                    Values.Add(uil.Value);
                }
            }
            mainGrid.UpdateOrderButtons<Universal_ItemList>();
        }

        private void Add(Controls.Universal_ItemList uil)
        {
            Values.Add(uil.Value);
            uil.deleteButton.Click += DeleteButton_Click;
            if (ReturnType == Universal_ItemList.ReturnType.Object || ReturnType == Universal_ItemList.ReturnType.Character || ReturnType == Controls.Universal_ItemList.ReturnType.Dialogue || ReturnType == Controls.Universal_ItemList.ReturnType.Vendor || ReturnType == Controls.Universal_ItemList.ReturnType.Quest)
            {
                uil.editButton.Click += EditButton_Click;
            }

            uil.Width = mainGrid.Width;

            uil.ShowMoveButtons |= ShowMoveButtons;

            if (uil.ShowMoveButtons)
            {
                uil.moveUpButton.Click += MoveUpButton_Click;
                uil.moveDownButton.Click += MoveDownButton_Click;
            }
            mainGrid.Children.Add(uil);
            mainGrid.UpdateOrderButtons<Universal_ItemList>();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedValue = Util.FindParent<Controls.Universal_ItemList>(sender as Button).Value;
            DialogResult = true;
            Close();
        }
        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.MoveUp((sender as UIElement).TryFindParent<Universal_ItemList>());
            UpdateValues();
        }
        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.MoveDown((sender as UIElement).TryFindParent<Universal_ItemList>());
            UpdateValues();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UpdateValues();
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClipboardManager.TryGetObject(ReturnType, out object obj))
                Add(new Universal_ItemList(obj, ShowMoveButtons));
        }
    }
}
