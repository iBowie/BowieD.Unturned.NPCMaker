using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Localization;
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
        public Universal_ListView(List<Controls.Universal_ItemList> listUil, Controls.Universal_ItemList.ReturnType returnType)
        {
            InitializeComponent();
            addButton.Content = LocalizationManager.Current.Interface[$"ListView_{returnType}_Add"];
            Title = LocalizationManager.Current.Interface[$"ListView_{returnType}_Title"];
            ReturnType = returnType;
            Values = new List<object>();
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
            List<object> newValues = new List<object>();
            foreach (UIElement ui in mainGrid.Children)
            {
                if (ui is Controls.Universal_ItemList uil)
                {
                    newValues.Add(uil.Value);
                }
            }
            Values = newValues;
        }

        public bool Localizable { get; set; }
        public List<object> Values { get; private set; }
        public Controls.Universal_ItemList.ReturnType ReturnType { get; private set; }

        public object SelectedValue { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ReturnType)
            {
                case Controls.Universal_ItemList.ReturnType.Condition:
                    Universal_ConditionEditor uce = new Universal_ConditionEditor();
                    if (uce.ShowDialog() == true)
                    {
                        Universal_ItemList a = new Controls.Universal_ItemList(uce.Result, Controls.Universal_ItemList.ReturnType.Condition, Localizable);
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
                    if (ure.ShowDialog() == true)
                    {
                        Universal_ItemList aa = new Controls.Universal_ItemList(ure.Result, Controls.Universal_ItemList.ReturnType.Reward, Localizable);
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
        }

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
            mainGrid.Children.Add(uil);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedValue = Util.FindParent<Controls.Universal_ItemList>(sender as Button).Value;
            DialogResult = true;
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UpdateValues();
        }
    }
}
