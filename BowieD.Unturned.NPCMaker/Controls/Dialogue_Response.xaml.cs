using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.ViewModels;
using DiscordRPC;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Response.xaml
    /// </summary>
    public partial class Dialogue_Response : DraggableUserControl, IHasOrderButtons, IHasDisplayedIndex
    {
        public Dialogue_Response(NPC.NPCResponse startResponse, DialogueTabViewModel parent)
        {
            InitializeComponent();
            Response = startResponse ?? new NPC.NPCResponse();
            mainText.Text = Response.mainText;
            txtBoxDialogueID.Value = Response.openDialogueId;
            txtBoxQuestID.Value = Response.openQuestId;
            txtBoxVendorID.Value = Response.openVendorId;

            ContextMenu mMenu = new ContextMenu();

            mMenu.Items.Add(ContextHelper.CreateCopyButton((sender, e) =>
            {
                parent.Save();

                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                Dialogue_Response target = context.PlacementTarget as Dialogue_Response;
                ClipboardManager.SetObject(ClipboardManager.DialogueReplyFormat, target.Response);
            }));
            mMenu.Items.Add(ContextHelper.CreateDuplicateButton((sender, e) =>
            {
                parent.Save();

                ContextMenu context = (sender as MenuItem).Parent as ContextMenu;
                Dialogue_Response target = context.PlacementTarget as Dialogue_Response;
                var cloned = target.Response.Clone();

                parent.AddResponse(new Dialogue_Response(cloned, parent));
            }));
            mMenu.Items.Add(ContextHelper.CreatePasteButton((sender, e) =>
            {
                if (ClipboardManager.TryGetObject(ClipboardManager.DialogueReplyFormat, out var obj) && obj is NPCResponse newReply)
                {
                    parent.AddResponse(new Dialogue_Response(newReply, parent));
                }
            }));

            this.ContextMenu = mMenu;

            if (Configuration.AppConfig.Instance.useOldStyleMoveUpDown)
            {
                dragRectGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                dragRect.MouseLeftButtonDown += DragControl_LMB_Down;
                dragRect.MouseLeftButtonUp += DragControl_LMB_Up;
                dragRect.MouseMove += DragControl_MouseMove;

                UpButton.Visibility = Visibility.Collapsed;
                DownButton.Visibility = Visibility.Collapsed;
            }

            mainText.ContextMenu = ContextHelper.CreateContextMenu(ContextHelper.EContextOption.Group_Dialogue | ContextHelper.EContextOption.Group_TextEdit);
        }

        public NPC.NPCResponse Response { get; private set; }

        public bool IsCollapsed { get; private set; }

        public void Expand()
        {
            expandedGrid.Visibility = Visibility.Visible;
            collapsedGrid.Visibility = Visibility.Collapsed;
        }
        public void Collapse()
        {
            expandedGrid.Visibility = Visibility.Collapsed;
            collapsedGrid.Visibility = Visibility.Visible;
            collapsedText.Text = TextUtil.Shortify(mainText.Text, 24);
        }

        public void RebuildResponse()
        {
            Response.mainText = mainText.Text;
            Response.openDialogueId = (ushort)txtBoxDialogueID.Value;
            Response.openQuestId = (ushort)txtBoxQuestID.Value;
            Response.openVendorId = (ushort)txtBoxVendorID.Value;
        }

        #region EVENTS
        private void EditRewardsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, true)).ToLimitedList(byte.MaxValue), Universal_ItemList.ReturnType.Reward);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}".Shortify(125),
                State = "Creating reward for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.rewards = ulv.Values.Cast<Reward>().ToLimitedList(byte.MaxValue);
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToLimitedList(byte.MaxValue), Universal_ItemList.ReturnType.Condition);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}".Shortify(125),
                State = "Creating condition for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.conditions = ulv.Values.Cast<Condition>().ToLimitedList(byte.MaxValue);
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void MainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Response.mainText = mainText.Text;
        }

        private void QuestSelect_Click(object sender, RoutedEventArgs e)
        {
            AssetPicker_Window apw = new AssetPicker_Window(typeof(GameQuestAsset));
            apw.Owner = MainWindow.Instance;
            if (apw.ShowDialog() == true)
            {
                txtBoxQuestID.Value = apw.SelectedAsset.ID;
            }
        }

        private void VendorSelect_Click(object sender, RoutedEventArgs e)
        {
            AssetPicker_Window apw = new AssetPicker_Window(typeof(GameVendorAsset));
            apw.Owner = MainWindow.Instance;
            if (apw.ShowDialog() == true)
            {
                txtBoxVendorID.Value = apw.SelectedAsset.ID;
            }
        }

        private void DialogueSelect_Click(object sender, RoutedEventArgs e)
        {
            AssetPicker_Window apw = new AssetPicker_Window(typeof(GameDialogueAsset));
            apw.Owner = MainWindow.Instance;
            if (apw.ShowDialog() == true)
            {
                txtBoxDialogueID.Value = apw.SelectedAsset.ID;
            }
        }

        private void ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.Messages.Count > 1)
            {
                Message_TreeView mtv = new Message_TreeView(Response.visibleIn, MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.Messages.Count);
                mtv.Owner = MainWindow.Instance;
                mtv.ShowDialog();
                if (mtv.DialogResult == true)
                {
                    int[] arr = mtv.AsIntArray;
                    Response.visibleIn = arr.Count() == 0 ? null : arr;
                }
            }
            else
            {
                App.NotificationManager.Notify(LocalizationManager.Current.Notification["Dialogue_Reply_Visiblity_TwoOrLessMessages"]);
            }
        }

        private void TxtBoxVendorID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openVendorId = (ushort)e.NewValue.Value;
            }
        }

        private void TxtBoxQuestID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openQuestId = (ushort)e.NewValue.Value;
            }
        }
        private void TxtBoxDialogueID_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (e.NewValue.HasValue)
            {
                Response.openDialogueId = (ushort)e.NewValue.Value;
            }
        }
        #endregion

        public UIElement UpButton => orderButtonUp;
        public UIElement DownButton => orderButtonDown;
        public Transform Transform => animateTransform;

        public override TranslateTransform DragRenderTransform => animateTransform;
        public override FrameworkElement DragControl => dragRect;

        public TextBlock IndexTextBlock => indexTextBlock;

        private void Collapse_Button_Click(object sender, RoutedEventArgs e)
        {
            Collapse();
        }
        private void Expand_Button_Click(object sender, RoutedEventArgs e)
        {
            Expand();
        }
    }
}
