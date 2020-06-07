using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using DiscordRPC;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;
using Reward = BowieD.Unturned.NPCMaker.NPC.Rewards.Reward;

namespace BowieD.Unturned.NPCMaker.Controls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_Response.xaml
    /// </summary>
    public partial class Dialogue_Response : UserControl
    {
        public Dialogue_Response(NPC.NPCResponse startResponse = null)
        {
            InitializeComponent();
            Response = startResponse ?? new NPC.NPCResponse();
            mainText.Text = Response.mainText;
            txtBoxDialogueID.Value = Response.openDialogueId;
            txtBoxQuestID.Value = Response.openQuestId;
            txtBoxVendorID.Value = Response.openVendorId;
        }

        public NPC.NPCResponse Response { get; private set; }
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
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, true)).ToList(), Universal_ItemList.ReturnType.Reward);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}",
                State = "Creating reward for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.rewards = ulv.Values.Cast<Reward>().ToArray();
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            Forms.Universal_ListView ulv = new Forms.Universal_ListView(Response.conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, true)).ToList(), Universal_ItemList.ReturnType.Condition);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.txtEditorName.Text ?? "without name"}",
                State = "Creating condition for a dialogue response"
            };
            (MainWindow.DiscordManager as DiscordRPC.DiscordManager)?.SendPresence(presence);
            ulv.Owner = MainWindow.Instance;
            ulv.ShowDialog();
            Response.conditions = ulv.Values.Cast<Condition>().ToArray();
            MainWindow.Instance.MainWindowViewModel.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void MainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Response.mainText = mainText.Text;
        }
        #endregion

        private void QuestSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.quests.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Quest);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxQuestID.Value = (select.SelectedValue as NPC.NPCQuest).id;
            }
        }

        private void VendorSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.vendors.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Vendor);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxVendorID.Value = (select.SelectedValue as NPC.NPCVendor).id;
            }
        }

        private void DialogueSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.CurrentProject.data.dialogues.Count() == 0)
            {
                return;
            }

            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Dialogue);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxDialogueID.Value = (select.SelectedValue as NPC.NPCDialogue).id;
            }
        }

        private void ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Messages.Count > 1)
            {
                Message_TreeView mtv = new Message_TreeView(Response.visibleIn, MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Messages.Count);
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

        public void UpdateOrderButtons()
        {
            StackPanel panel = MainWindow.Instance.dialoguePlayerRepliesGrid;
            int index = IndexInPanel;
            if (index == 0)
            {
                orderButtonUp.IsEnabled = false;
            }
            else if (index >= 1)
            {
                orderButtonUp.IsEnabled = true;
            }

            if (index == panel.Children.Count - 2)
            {
                orderButtonDown.IsEnabled = false;
            }
            else if (index < panel.Children.Count - 2)
            {
                orderButtonDown.IsEnabled = true;
            }
        }

        private int IndexInPanel
        {
            get
            {
                for (int k = 0; k < MainWindow.Instance.dialoguePlayerRepliesGrid.Children.Count; k++)
                {
                    if (MainWindow.Instance.dialoguePlayerRepliesGrid.Children[k] == this)
                    {
                        return k;
                    }
                }
                return -1;
            }
        }

        private void OrderButtonDown_Click(object sender, RoutedEventArgs e)
        {
            int index = IndexInPanel;
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.responses.Remove(Response);
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.responses.Insert(index + 1, Response);
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.UpdateResponses();
        }

        private void OrderButtonUp_Click(object sender, RoutedEventArgs e)
        {
            int index = IndexInPanel;
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.responses.Remove(Response);
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.Dialogue.responses.Insert(index - 1, Response);
            MainWindow.Instance.MainWindowViewModel.DialogueTabViewModel.UpdateResponses();
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
    }
}
