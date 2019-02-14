using BowieD.Unturned.NPCMaker.NPC;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Linq;

namespace BowieD.Unturned.NPCMaker.BetterForms
{
    /// <summary>
    /// Логика взаимодействия для Universal_VendorItemEditor.xaml
    /// </summary>
    public partial class Universal_VendorItemEditor : Window
    {
        public Universal_VendorItemEditor(VendorItem startItem = null)
        {
            InitializeComponent();

            double scale = Config.Configuration.Properties.scale;
            stateA *= scale;
            stateB *= scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            Width *= scale;
            Height = stateB;
            typeBox.SelectedIndex = 0;
            sellBox.SelectedIndex = 0;
            Result = startItem ?? new VendorItem();
            txtBoxCost.Value = Result.cost;
            txtBoxID.Value = Result.id;
            txtBoxSpawnpoint.Text = Result.spawnPointID.ToString();
            foreach (ComboBoxItem cbi in typeBox.Items)
            {
                ItemType it = (ItemType)cbi.Tag;
                if (it == Result.type)
                {
                    typeBox.SelectedItem = cbi;
                    break;
                }
            }
            foreach (ComboBoxItem cbi in sellBox.Items)
            {
                if ((cbi.Tag.ToString() == "BUY" && Result.isBuy) || (cbi.Tag.ToString() == "SELL" && !Result.isBuy))
                {
                    sellBox.SelectedItem = cbi;
                    break;
                }
            }
        }

        private bool disableTypeChange { get; set; }
        private bool disableSellTypeChange { get; set; }
        public VendorItem Result { get; private set; }

        private double stateA = 326;
        private double stateB = 296;

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            if (Selected_ItemType == ItemType.ITEM)
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation(1, 0, new Duration(new System.TimeSpan(0, 0, 0, 0, 500)));
                DoubleAnimation heightAnimation = new DoubleAnimation(stateA, stateB, new Duration(new System.TimeSpan(0, 0, 0, 0, 500)));
                BeginAnimation(HeightProperty, heightAnimation);
                txtBoxSpawnpoint.BeginAnimation(OpacityProperty, opacityAnimation);
                labelSpawnpoint.BeginAnimation(OpacityProperty, opacityAnimation);
                sellBox.IsEnabled = true;
                txtBoxSpawnpoint.Text = "";
            }
            else
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, new Duration(new System.TimeSpan(0, 0, 0, 0, 500)));
                DoubleAnimation heightAnimation = new DoubleAnimation(stateB, stateA, new Duration(new System.TimeSpan(0, 0, 0, 0, 500)));
                BeginAnimation(HeightProperty, heightAnimation);
                txtBoxSpawnpoint.BeginAnimation(OpacityProperty, opacityAnimation);
                labelSpawnpoint.BeginAnimation(OpacityProperty, opacityAnimation);
                sellBox.IsEnabled = false;
                sellBox.SelectedIndex = 1;
            }
        }

        private ItemType Selected_ItemType => typeBox.SelectedItem is ComboBoxItem cbi && cbi.Tag is ItemType it ? it : ItemType.ITEM;
        private bool IsBuy => sellBox.SelectedItem is ComboBoxItem cbi && cbi.Tag.ToString() == "BUY";

        private void EditConditions_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(Result.conditions.Select(d => new BetterControls.Universal_ItemList(d, BetterControls.Universal_ItemList.ReturnType.Condition, false)).ToList(), BetterControls.Universal_ItemList.ReturnType.Condition);
            ulv.ShowDialog();
            Result.conditions = ulv.Values.Cast<NPC.Condition>().ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Result.id = (ushort)txtBoxID.Value;
                Result.type = Selected_ItemType;
                if (Result.type == ItemType.VEHICLE)
                    Result.spawnPointID = ushort.Parse(txtBoxSpawnpoint.Text);
                Result.isBuy = IsBuy;
                Result.cost = (uint)txtBoxCost.Value;
                if (Result.conditions == null)
                    Result.conditions = new List<NPC.Condition>();
                DialogResult = true;
                Close();
            }
            catch { }
        }
    }
}
