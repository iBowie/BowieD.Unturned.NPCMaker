using BowieD.Unturned.NPCMaker.BetterControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_ListView.xaml
    /// </summary>
    public partial class Universal_ListView : Window
    {
        public Universal_ListView(List<BetterControls.Universal_ItemList> listUil, BetterControls.Universal_ItemList.ReturnType returnType)
        {
            InitializeComponent();
            addButton.Content = MainWindow.Localize($"listView_Add_{returnType.ToString()}");
            Title = MainWindow.Localize($"listView_Title_{returnType.ToString()}");
            ReturnType = returnType;
            Values = new List<object>();
            foreach (BetterControls.Universal_ItemList uil in listUil)
            {
                Add(uil);
            }
            double scale = Config.Configuration.Properties.scale;
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
                var ll = ui as BetterControls.Universal_ItemList;
                if (ll.Equals(Util.FindParent<BetterControls.Universal_ItemList>(sender as UIElement)))
                {
                    mainGrid.Children.Remove(ll);
                    break;
                }
            }
            List<object> newValues = new List<object>();
            foreach (UIElement ui in mainGrid.Children)
            {
                if (ui is BetterControls.Universal_ItemList uil)
                {
                    newValues.Add(uil.Value);
                }
            }
            Values = newValues;
        }

        public bool Localizable { get; set; }
        public List<object> Values { get; private set; }
        public BetterControls.Universal_ItemList.ReturnType ReturnType { get; private set; }

        public object SelectedValue { get; private set; }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            switch (ReturnType)
            {
                case BetterControls.Universal_ItemList.ReturnType.Condition:
                    Universal_ConditionEditor uce = new Universal_ConditionEditor();
                    if (uce.ShowDialog() == true)
                    {
                        var a = new BetterControls.Universal_ItemList(uce.Result, BetterControls.Universal_ItemList.ReturnType.Condition, Localizable);
                        Add(a);
                    }
                    break;
                case BetterControls.Universal_ItemList.ReturnType.Dialogue:
                    SelectedValue = new NPC.NPCDialogue();
                    DialogResult = true;
                    Close();
                    break;
                case BetterControls.Universal_ItemList.ReturnType.Vendor:
                    SelectedValue = new NPC.NPCVendor();
                    DialogResult = true;
                    Close();
                    break;
                case BetterControls.Universal_ItemList.ReturnType.Reward:
                    Universal_RewardEditor ure = new Universal_RewardEditor();
                    if (ure.ShowDialog() == true)
                    {
                        var aa = new BetterControls.Universal_ItemList(ure.Result, BetterControls.Universal_ItemList.ReturnType.Reward, Localizable);
                        Add(aa);
                    }
                    break;
                case BetterControls.Universal_ItemList.ReturnType.Quest:
                    SelectedValue = new NPC.NPCQuest();
                    DialogResult = true;
                    Close();
                    break;
                case Universal_ItemList.ReturnType.Character:
                    SelectedValue = new NPC.NPCCharacter();
                    DialogResult = true;
                    Close();
                    break;
                //case Universal_ItemList.ReturnType.Object:
                //    SelectedValue = new NPC.NPCObject();
                //    DialogResult = true;
                //    Close();
                //    break;
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

        private void Add(BetterControls.Universal_ItemList uil)
        {
            Values.Add(uil.Value);
            uil.deleteButton.Click += DeleteButton_Click;
            if (ReturnType == Universal_ItemList.ReturnType.Object || ReturnType == Universal_ItemList.ReturnType.Character || ReturnType == BetterControls.Universal_ItemList.ReturnType.Dialogue || ReturnType == BetterControls.Universal_ItemList.ReturnType.Vendor || ReturnType == BetterControls.Universal_ItemList.ReturnType.Quest)
                uil.editButton.Click += EditButton_Click;
            uil.Width = mainGrid.Width;
            mainGrid.Children.Add(uil);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedValue = Util.FindParent<BetterControls.Universal_ItemList>(sender as Button).Value;
            DialogResult = true;
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UpdateValues();
        }
    }
}
