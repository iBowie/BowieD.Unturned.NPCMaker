using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.GameIntegration.Devkit;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для Universal_VendorItemEditor.xaml
    /// </summary>
    public partial class Universal_VendorItemEditor : Window
    {
        private bool ignoreAnimation = true;
        private readonly NPCVendor _vendor;

        public Universal_VendorItemEditor(NPCVendor vendor, VendorItem startItem = null)
        {
            InitializeComponent();

            _vendor = vendor;

            bool 
                allowBuy = vendor.BuyItems.Count < byte.MaxValue, 
                allowSell = vendor.SellItems.Count < byte.MaxValue;

            ignoreAnimation = true;

            double scale = AppConfig.Instance.scale;
            gridScale.ScaleX = scale;
            gridScale.ScaleY = scale;
            Width *= scale;
            Height *= scale;
            typeBox.SelectedIndex = 0;
            sellBox.SelectedIndex = 0;
            Result = startItem ?? new VendorItem() { isBuy = !isSellSelected, type = lastType };
            txtBoxCost.Value = Result.cost;
            txtBoxID.Value = Result.id;
            txtBoxSpawnpoint.Text = Result.spawnPointID;

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

            ContextMenu cmenu = new ContextMenu();
            MenuItem selectItem = ContextHelper.CreateSelectAssetButton(typeof(GameItemAsset), (asset) =>
            {
                txtBoxID.Value = asset.ID;
            }, "Control_SelectAsset_Item", MahApps.Metro.IconPacks.PackIconMaterialKind.Archive);
            MenuItem selectVehicle = ContextHelper.CreateSelectAssetButton(typeof(GameVehicleAsset), (asset) =>
            {
                txtBoxID.Value = asset.ID;
            }, "Control_SelectAsset_Vehicle", MahApps.Metro.IconPacks.PackIconMaterialKind.Car);

            selectItem.Command = new AdvancedCommand(() => { }, (obj) =>
            {
                return Selected_ItemType == ItemType.ITEM;
            });
            selectVehicle.Command = new AdvancedCommand(() => { }, (obj) =>
            {
                return Selected_ItemType == ItemType.VEHICLE;
            });

            saveButton.Command = new AdvancedCommand(() =>
            {
                try
                {
                    Result.id = (ushort)txtBoxID.Value;
                    Result.type = Selected_ItemType;
                    if (Result.type == ItemType.VEHICLE)
                    {
                        Result.spawnPointID = txtBoxSpawnpoint.Text;
                    }

                    Result.isBuy = IsBuy;
                    Result.cost = (uint)txtBoxCost.Value;
                    if (Result.conditions == null)
                    {
                        Result.conditions = new List<Condition>();
                    }

                    isSellSelected = !IsBuy;
                    lastType = Selected_ItemType;
                    DialogResult = true;
                    Close();
                }
                catch { }
            }, (p) => 
            {
                if (IsBuy)
                {
                    if (allowBuy)
                    {
                        tooManyItemsLabel.Visibility = Visibility.Collapsed;
                        return true;
                    }
                    else
                    {
                        tooManyItemsLabel.Visibility = Visibility.Visible;
                        return false;
                    }
                }
                else
                {
                    if (allowSell)
                    {
                        tooManyItemsLabel.Visibility = Visibility.Collapsed;
                        return true;
                    }
                    else
                    {
                        tooManyItemsLabel.Visibility = Visibility.Visible;
                        return false;
                    }
                }
            });

            cmenu.Items.Add(selectItem);
            cmenu.Items.Add(selectVehicle);

            txtBoxID.ContextMenu = cmenu;

            ContextMenu cmenuSpawnpoint = new ContextMenu();

            cmenuSpawnpoint.Items.Add(ContextHelper.CreateSelectAssetButton(typeof(Spawnpoint), (iap) =>
            {
                txtBoxSpawnpoint.Text = iap.Name;
            }, "Control_SelectAsset_DKSpawnpoint", MahApps.Metro.IconPacks.PackIconMaterialKind.MapMarker));

            txtBoxSpawnpoint.ContextMenu = cmenuSpawnpoint;
        }
        public VendorItem Result { get; private set; }

        public AnimationTimeline DisappearAnimation(FrameworkElement element, double current)
        {
            var da = new DoubleAnimation(current, 0, new Duration(new TimeSpan(0, 0, 1)));
            da.Completed += (sender, e) =>
            {
                element.Visibility = Visibility.Collapsed;
            };
            return da;
        }
        public AnimationTimeline AppearAnimation(FrameworkElement element, double current)
        {
            element.Visibility = Visibility.Visible;
            var da = new DoubleAnimation(current, 1, new Duration(new TimeSpan(0, 0, 1)));
            return da;
        }

        private void TypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();

            if (e.AddedItems.Count == 0)
            {
                return;
            }

            if (Selected_ItemType == ItemType.ITEM)
            {
                if (AppConfig.Instance.animateControls && !ignoreAnimation)
                {
                    txtBoxSpawnpoint.BeginAnimation(OpacityProperty, DisappearAnimation(txtBoxSpawnpoint, txtBoxSpawnpoint.Opacity));
                    labelSpawnpoint.BeginAnimation(OpacityProperty, DisappearAnimation(labelSpawnpoint, labelSpawnpoint.Opacity));
                }
                else
                {
                    ignoreAnimation = false;

                    labelSpawnpoint.Opacity = 0;
                    labelSpawnpoint.Visibility = Visibility.Collapsed;

                    txtBoxSpawnpoint.Opacity = 0;
                    txtBoxSpawnpoint.Visibility = Visibility.Collapsed;
                }
                txtBoxSpawnpoint.IsHitTestVisible = false;
                sellBox.IsEnabled = true;
                txtBoxSpawnpoint.Text = "";
            }
            else
            {
                if (AppConfig.Instance.animateControls && !ignoreAnimation)
                {
                    txtBoxSpawnpoint.BeginAnimation(OpacityProperty, AppearAnimation(txtBoxSpawnpoint, txtBoxSpawnpoint.Opacity));
                    labelSpawnpoint.BeginAnimation(OpacityProperty, AppearAnimation(labelSpawnpoint, labelSpawnpoint.Opacity));
                }
                else
                {
                    ignoreAnimation = false;

                    labelSpawnpoint.Opacity = 1;
                    labelSpawnpoint.Visibility = Visibility.Visible;

                    txtBoxSpawnpoint.Opacity = 1;
                    txtBoxSpawnpoint.Visibility = Visibility.Visible;
                }
                txtBoxSpawnpoint.IsHitTestVisible = true;
                sellBox.IsEnabled = false;
                sellBox.SelectedIndex = 1;
            }
        }

        private ItemType Selected_ItemType => typeBox.SelectedItem is ComboBoxItem cbi && cbi.Tag is ItemType it ? it : ItemType.ITEM;
        private bool IsBuy => sellBox.SelectedItem is ComboBoxItem cbi && cbi.Tag.ToString() == "BUY";

        private void EditConditions_Click(object sender, RoutedEventArgs e)
        {
            Universal_ListView ulv = new Universal_ListView(Result.conditions.Select(d => new Controls.Universal_ItemList(d, Controls.Universal_ItemList.ReturnType.Condition, true)).ToList(), Controls.Universal_ItemList.ReturnType.Condition);
            ulv.Owner = this;
            ulv.ShowDialog();
            Result.conditions = ulv.Values.Cast<Condition>().ToList();
        }

        private static bool isSellSelected = false;
        private static ItemType lastType = ItemType.ITEM;
    }
}
