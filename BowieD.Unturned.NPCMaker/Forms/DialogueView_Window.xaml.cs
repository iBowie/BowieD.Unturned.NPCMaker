using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.IconPacks;
using System;
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
        public DialogueView_Window(NPCCharacter character, NPCDialogue dialogue, Simulation simulation, NPCDialogue prev = null)
        {
            InitializeComponent();

            this.Character = character;
            this.Dialogue = dialogue;
            this.Simulation = simulation;
            this.Previous = prev;

            this.Loaded += (sender, e) =>
            {
                SimulationView_Window svw = new SimulationView_Window(this, Simulation);
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

        private NPCMessage lastMessage;
        private int lastMessageId;
        private int lastPage = 0;
        private bool canDisplayNextPage = true;

        public void DisplayPage(NPCMessage message, int i, int page)
        {
            Simulation.OnPropertyChanged("");

            mainText.Text = FormatText(message.pages[page]);

            if (page == message.pages.Count - 1)
            {
                canDisplayNextPage = false;

                foreach (var res in Dialogue.responses)
                {
                    if ((res.VisibleInAll || res.visibleIn.Length <= i || res.visibleIn[i] == 1) && res.conditions.All(d => d.Check(Simulation)))
                    {
                        Border border = new Border()
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(4),
                            Margin = new Thickness(0, 2.5, 0, 2.5)
                        };

                        border.PreviewMouseLeftButtonDown += (sender, e) =>
                        {
                            foreach (var c in res.conditions)
                                c.Apply(Simulation);
                            foreach (var r in res.rewards)
                                r.Give(Simulation);

                            bool shouldClose = true;
                            bool shouldContinue = false;

                            if (res.openVendorId > 0)
                            {
                                shouldClose = false;
                            }
                            else if (res.openQuestId > 0)
                            {
                                shouldClose = false;

                                var questAsset = MainWindow.CurrentProject.data.quests.Single(d => d.id == res.openQuestId);

                                QuestView_Window qvw = new QuestView_Window(Character, Simulation, questAsset);
                                if (qvw.ShowDialog() == true)
                                {
                                    shouldContinue = true;
                                }
                            }
                            else
                            {
                                shouldContinue = true;
                            }

                            if (shouldContinue)
                            {
                                if (res.openDialogueId > 0)
                                {
                                    Previous = Dialogue;

                                    var next = MainWindow.CurrentProject.data.dialogues.Single(d => d.id == res.openDialogueId);

                                    Dialogue = next;

                                    Display();
                                }
                                else if (shouldClose)
                                {
                                    Close();
                                }
                            }
                        };

                        Grid g = new Grid();


                        TextBlock tb = new TextBlock()
                        {
                            Text = FormatText(res.mainText)
                        };

                        Label l = new Label()
                        {
                            Content = tb,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(5)
                        };

                        g.Children.Add(l);

                        PackIconMaterial icon = null;

                        if (res.openVendorId > 0)
                        {
                            icon = new PackIconMaterial()
                            {
                                Kind = PackIconMaterialKind.ShoppingOutline
                            };
                        }
                        else if (res.openQuestId > 0)
                        {
                            icon = new PackIconMaterial()
                            {
                                Kind = PackIconMaterialKind.Exclamation
                            };
                        }

                        if (icon != null)
                        {
                            icon.VerticalAlignment = VerticalAlignment.Center;
                            icon.HorizontalAlignment = HorizontalAlignment.Left;
                            icon.Margin = new Thickness(5);

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

            npcNameText.Text = FormatText(Character.displayName) ?? string.Empty;

            for (int i = 0; i < Dialogue.messages.Count; i++)
            {
                NPCMessage msg = Dialogue.messages[i];
                if (msg.conditions.All(d => d.Check(Simulation)))
                {
                    foreach (var c in msg.conditions)
                        c.Apply(Simulation);
                    foreach (var r in msg.rewards)
                        r.Give(Simulation);

                    lastMessageId = i;
                    lastPage = 0;
                    lastMessage = msg;

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
                if (Previous != null)
                {
                    Dialogue = Previous;

                    Previous = null;

                    Display();
                }
            }
        }

        private string FormatText(string raw)
        {
            return SimulationTool.ReplacePlaceholders(Character, Simulation, raw);
        }
    }
}
