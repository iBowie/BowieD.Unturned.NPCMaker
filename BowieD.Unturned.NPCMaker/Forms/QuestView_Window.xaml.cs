using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for QuestView_Window.xaml
    /// </summary>
    public partial class QuestView_Window : Window
    {
        public QuestView_Window(NPCCharacter character, Simulation simulation, NPCQuest quest)
        {
            InitializeComponent();

            this.Quest = quest;

            title.Text = SimulationTool.ReplacePlaceholders(character, simulation, quest.title);
            desc.Text = SimulationTool.ReplacePlaceholders(character, simulation, quest.description);

            foreach (var c in quest.conditions)
            {
                string text = c.FormatCondition(simulation);

                if (text == null) continue;

                Border b = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4)
                };

                TextBlock tb = new TextBlock()
                {
                    Text = text
                };

                Label l = new Label()
                {
                    Content = tb
                };

                b.Child = l;

                goalsPanel.Children.Add(b);
            }

            foreach (var r in quest.rewards)
            {
                string text = r.FormatReward(simulation);

                if (text == null) continue;

                Border b = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4)
                };

                TextBlock tb = new TextBlock()
                {
                    Text = text
                };

                Label l = new Label()
                {
                    Content = tb
                };

                b.Child = l;

                rewardsPanel.Children.Add(b);
            }
        }

        public NPCQuest Quest { get; }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
