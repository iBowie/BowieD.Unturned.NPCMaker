using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Markup;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.IconPacks;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for DialogueView_Window.xaml
    /// </summary>
    public partial class DialogueView_Window : Window
    {
        static IMarkup formatter = new RichText();

        public DialogueView_Window(NPCCharacter character, NPCDialogue dialogue, Simulation simulation, NPCDialogue prev = null)
        {
            InitializeComponent();

            Character = character;
            Dialogue = dialogue;
            Simulation = simulation;
            Previous = prev;
            Start = dialogue;

            Loaded += (sender, e) =>
            {
                SimulationView_Window svw = new SimulationView_Window(this, Simulation);
                svw.Owner = this;
                svw.Show();
                Closing += (sender2, e2) =>
                {
                    svw.Close();
                };
            };
        }

        public NPCCharacter Character { get; }
        public NPCDialogue Dialogue { get; private set; }
        public Simulation Simulation { get; }
        public NPCDialogue Previous { get; private set; }
        public NPCDialogue Start { get; }

        private NPCMessage lastMessage;
        private int lastMessageId;
        private int lastPage = 0;
        private bool canDisplayNextPage => lastPage < lastMessage.pages.Count - 1;

        public void DisplayPage(NPCMessage message, int i, int page)
        {
            Simulation.OnPropertyChanged("");

            formatter.Markup(mainText, FormatText(message.pages[page]));

            lastPage = page;

            if (!canDisplayNextPage)
            {
                foreach (NPCResponse res in Dialogue.Responses)
                {
                    if ((res.VisibleInAll || res.visibleIn.Length <= i || res.visibleIn[i] == 1) && res.conditions.All(d => d.Check(Simulation)))
                    {
                        Border border = new Border()
                        {
                            BorderBrush = App.Current.Resources["AccentColor"] as Brush,
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(4),
                            Margin = new Thickness(0, 2.5, 0, 2.5)
                        };

                        border.PreviewMouseLeftButtonDown += (sender, e) =>
                        {
                            bool shouldClose = true;

                            if (res.openQuestId > 0)
                            {
                                shouldClose = false;

                                NPCQuest questAsset = MainWindow.CurrentProject.data.quests.Single(d => d.id == res.openQuestId);

                                Quest_Status questStatus = Simulation.GetQuestStatus(questAsset.id);

                                QuestView_Window.EMode _mode;

                                switch (questStatus)
                                {
                                    case Quest_Status.Ready:
                                        _mode = QuestView_Window.EMode.END_QUEST;
                                        break;
                                    default:
                                        _mode = QuestView_Window.EMode.BEGIN_QUEST;
                                        break;
                                }

                                QuestView_Window qvw = new QuestView_Window(Character, Simulation, questAsset, _mode);
                                if (qvw.ShowDialog() == true)
                                {
                                    foreach (NPC.Conditions.Condition c in res.conditions)
                                    {
                                        c.Apply(Simulation);
                                    }

                                    foreach (NPC.Rewards.Reward r in res.rewards)
                                    {
                                        r.Give(Simulation);
                                    }

                                    if (res.openDialogueId > 0)
                                    {
                                        Previous = Start;

                                        NPCDialogue next = MainWindow.CurrentProject.data.dialogues.Single(d => d.ID == res.openDialogueId);

                                        Dialogue = next;

                                        Display();
                                    }
                                }

                                return;
                            }
                            else if (res.openVendorId > 0)
                            {
                                shouldClose = false;

                                NPCVendor vendorAsset = MainWindow.CurrentProject.data.vendors.Single(d => d.id == res.openVendorId);

                                VendorView_Window qvw = new VendorView_Window(Character, Simulation, vendorAsset);

                                qvw.ShowDialog();

                                foreach (NPC.Conditions.Condition c in res.conditions)
                                {
                                    c.Apply(Simulation);
                                }

                                foreach (NPC.Rewards.Reward r in res.rewards)
                                {
                                    r.Give(Simulation);
                                }

                                if (res.openDialogueId > 0)
                                {
                                    Previous = Start;

                                    NPCDialogue next = MainWindow.CurrentProject.data.dialogues.Single(d => d.ID == res.openDialogueId);

                                    Dialogue = next;

                                    Display();
                                }

                                return;
                            }

                            foreach (NPC.Conditions.Condition c in res.conditions)
                            {
                                c.Apply(Simulation);
                            }

                            foreach (NPC.Rewards.Reward r in res.rewards)
                            {
                                r.Give(Simulation);
                            }

                            if (res.openDialogueId > 0)
                            {
                                Previous = Dialogue;

                                NPCDialogue next = MainWindow.CurrentProject.data.dialogues.Single(d => d.ID == res.openDialogueId);

                                Dialogue = next;

                                Display();
                            }
                            else if (shouldClose)
                            {
                                Close();
                            }
                        };

                        Grid g = new Grid();


                        TextBlock tb = new TextBlock();
                        formatter.Markup(tb, FormatText(res.mainText));

                        Label l = new Label()
                        {
                            Content = tb,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(5)
                        };

                        g.Children.Add(l);

                        PackIconMaterial icon = null;

                        if (res.openQuestId > 0)
                        {
                            switch (Simulation.GetQuestStatus(res.openQuestId))
                            {
                                case Quest_Status.Ready:
                                    icon = new PackIconMaterial()
                                    {
                                        Kind = PackIconMaterialKind.Help
                                    };
                                    break;
                                default:
                                    icon = new PackIconMaterial()
                                    {
                                        Kind = PackIconMaterialKind.Exclamation
                                    };
                                    break;
                            }
                        }
                        else if (res.openVendorId > 0)
                        {
                            icon = new PackIconMaterial()
                            {
                                Kind = PackIconMaterialKind.ShoppingOutline
                            };
                        }

                        if (icon != null)
                        {
                            icon.VerticalAlignment = VerticalAlignment.Center;
                            icon.HorizontalAlignment = HorizontalAlignment.Left;
                            icon.Margin = new Thickness(5);
                            icon.Foreground = App.Current.Resources["AccentColor"] as Brush;

                            g.Children.Add(icon);
                        }

                        border.Child = g;
                        responsesPanel.Children.Add(border);
                    }
                }
            }
        }
        public void Display()
        {
            Simulation.OnPropertyChanged("");

            mainText.Text = string.Empty;
            responsesPanel.Children.Clear();

            formatter.Markup(npcNameText, FormatText(Character.DisplayName) ?? string.Empty);

            for (int i = 0; i < Dialogue.Messages.Count; i++)
            {
                NPCMessage msg = Dialogue.Messages[i];
                if (msg.conditions.All(d => d.Check(Simulation)))
                {
                    foreach (NPC.Conditions.Condition c in msg.conditions)
                    {
                        c.Apply(Simulation);
                    }

                    foreach (NPC.Rewards.Reward r in msg.rewards)
                    {
                        r.Give(Simulation);
                    }

                    lastMessageId = i;
                    lastPage = 0;
                    lastMessage = msg;

                    if (msg.prev != 0)
                        Previous = MainWindow.CurrentProject.data.dialogues.SingleOrDefault(d => d.ID == msg.prev);

                    DisplayPage(msg, i, 0);

                    break;
                }
            }
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (canDisplayNextPage)
            {
                DisplayPage(lastMessage, lastMessageId, ++lastPage);
            }
            else
            {
                if (responsesPanel.Children.Count == 0)
                {
                    if (Previous != null)
                    {
                        Dialogue = Previous;

                        Previous = null;

                        Display();
                    }
                }
            }
        }

        private string FormatText(string raw)
        {
            return SimulationTool.ReplacePlaceholders(Character, Simulation, raw);
        }
    }
}
