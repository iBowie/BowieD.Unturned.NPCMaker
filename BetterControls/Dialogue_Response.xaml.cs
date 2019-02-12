using BowieD.Unturned.NPCMaker.BetterForms;
using DiscordRPC;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.BetterControls
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
            txtBoxDialogueID.Text = Response.openDialogueId.ToString();
            txtBoxQuestID.Text = Response.openQuestId.ToString();
            txtBoxVendorID.Text = Response.openVendorId.ToString();
        }
        
        public NPC.NPCResponse Response { get; private set; }

        #region EVENTS
        private void EditRewardsButton_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(Response.rewards.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Reward, false)).ToList(), Universal_ItemList.ReturnType.Reward);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.Inputted_EditorName}",
                State = "Creating reward for a dialogue response"
            };
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            ulv.ShowDialog();
            Response.rewards = ulv.Values.Cast<NPC.Reward>().ToArray();
            MainWindow.Instance.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void EditConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            BetterForms.Universal_ListView ulv = new BetterForms.Universal_ListView(Response.conditions.Select(d => new Universal_ItemList(d, Universal_ItemList.ReturnType.Condition, false)).ToList(), Universal_ItemList.ReturnType.Condition);
            RichPresence presence = new RichPresence
            {
                Details = $"Editing NPC {MainWindow.Instance.Inputted_EditorName}",
                State = "Creating condition for a dialogue response"
            };
            (MainWindow.DiscordWorker as DiscordRPC.DiscordWorker)?.SendPresence(presence);
            ulv.ShowDialog();
            Response.conditions = ulv.Values.Cast<NPC.Condition>().ToArray();
            MainWindow.Instance.TabControl_SelectionChanged(MainWindow.Instance.mainTabControl, null);
        }
        private void MainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Response.mainText = mainText.Text;
        }
        private void TxtBoxDialogueID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBoxDialogueID.Text.Count(d => !char.IsNumber(d)) == 0 && ushort.TryParse(txtBoxDialogueID.Text, out ushort resultedId))
                Response.openDialogueId = resultedId;
        }
        private void TxtBoxVendorID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBoxVendorID.Text.Count(d => !char.IsNumber(d)) == 0 && ushort.TryParse(txtBoxVendorID.Text, out ushort resultedId))
                Response.openVendorId = resultedId;
        }
        private void TxtBoxQuestID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBoxQuestID.Text.Count(d => !char.IsNumber(d)) == 0 && ushort.TryParse(txtBoxQuestID.Text, out ushort resultedId))
                Response.openQuestId= resultedId;
        }
        #endregion

        private void QuestSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.quests.Count() == 0)
                return;
            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Quest);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxQuestID.Text = (select.SelectedValue as NPC.NPCQuest).id.ToString();
            }
        }

        private void VendorSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.vendors.Count() == 0)
                return;
            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Vendor);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxVendorID.Text = (select.SelectedValue as NPC.NPCVendor).id.ToString();
            }
        }

        private void DialogueSelect_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.dialogues.Count() == 0)
                return;
            Universal_Select select = new Universal_Select(Universal_ItemList.ReturnType.Dialogue);
            select.ShowDialog();
            if (select.DialogResult == true)
            {
                txtBoxDialogueID.Text = (select.SelectedValue as NPC.NPCDialogue).id.ToString();
            }
        }

        private void ChangeVisibilityButton_Click(object sender, RoutedEventArgs e)
        {
            Message_TreeView mtv = new Message_TreeView(Response.visibleIn, MainWindow.Instance.CurrentDialogue.MessagesAmount);
            mtv.ShowDialog();
            if (mtv.SaveApply)
            {
                var arr = mtv.AsIntArray;
                Response.visibleIn = arr.Count() == 0 ? null : arr;
            }
        }

        public void UpdateOrderButtons()
        {
            var panel = MainWindow.Instance.dialoguePlayerRepliesGrid;
            int index = IndexInPanel;
            if (index == 0)
                orderButtonUp.IsEnabled = false;
            else if (index >= 1)
                orderButtonUp.IsEnabled = true;
            if (index == panel.Children.Count - 2)
                orderButtonDown.IsEnabled = false;
            else if (index < panel.Children.Count - 2)
                orderButtonDown.IsEnabled = true;
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
            var panel = MainWindow.Instance.dialoguePlayerRepliesGrid;
            int index = IndexInPanel;
            Dialogue_Response current = this;
            Dialogue_Response changeTo = panel.Children[index + 1] as Dialogue_Response;
            panel.Children.RemoveAt(index);
            panel.Children.Insert(index + 1, current);
            foreach (UIElement uie in panel.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
        }

        private void OrderButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var panel = MainWindow.Instance.dialoguePlayerRepliesGrid;
            int index = IndexInPanel;
            Dialogue_Response current = this;
            Dialogue_Response changeTo = panel.Children[index - 1] as Dialogue_Response;
            panel.Children.RemoveAt(index-1);
            panel.Children.Insert(index, changeTo);
            foreach (UIElement uie in panel.Children)
            {
                if (uie is Dialogue_Response dr)
                {
                    dr.UpdateOrderButtons();
                }
            }
        }
    }
}
