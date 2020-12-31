using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Controls;
using BowieD.Unturned.NPCMaker.Markup;
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
        static IMarkup formatter = new RichText();

        public QuestView_Window(NPCCharacter character, Simulation simulation, NPCQuest quest, EMode mode = EMode.PREVIEW)
        {
            InitializeComponent();

            Quest = quest;

            switch (mode)
            {
                case EMode.PREVIEW:
                    acceptButton.IsEnabled = false;
                    acceptButton.Visibility = Visibility.Collapsed;
                    continueButton.IsEnabled = false;
                    continueButton.Visibility = Visibility.Collapsed;
                    break;
                case EMode.BEGIN_QUEST:
                    continueButton.IsEnabled = false;
                    continueButton.Visibility = Visibility.Collapsed;
                    break;
                case EMode.END_QUEST:
                    acceptButton.IsEnabled = false;
                    acceptButton.Visibility = Visibility.Collapsed;
                    break;
            }

            formatter.Markup(title, SimulationTool.ReplacePlaceholders(character, simulation, quest.Title));
            formatter.Markup(desc, SimulationTool.ReplacePlaceholders(character, simulation, quest.description));

            foreach (NPC.Conditions.Condition c in quest.conditions)
            {
                string text = c.FormatCondition(simulation);

                if (text == null)
                {
                    continue;
                }

                Grid g = new Grid();

                Border b = new Border()
                {
                    BorderBrush = App.Current.Resources["AccentColor"] as Brush,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4)
                };

                TextBlock tb = new TextBlock();
                formatter.Markup(tb, text);

                Label l = new Label()
                {
                    Content = tb
                };

                Image i = new Image()
                {
                    Width = 26,
                    Height = 26,
                    Margin = new Thickness(5)
                };

                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                g.ColumnDefinitions.Add(new ColumnDefinition());

                g.Children.Add(i);
                g.Children.Add(l);
                Grid.SetColumn(l, 1);

                if (c is IUIL_Icon uIL && uIL.UpdateIcon(out var img))
                {
                    i.Visibility = Visibility.Visible;
                    i.Source = img;
                }
                else
                {
                    i.Visibility = Visibility.Collapsed;
                }

                b.Child = g;

                goalsPanel.Children.Add(b);
            }

            foreach (NPC.Rewards.Reward r in quest.rewards)
            {
                string text = r.FormatReward(simulation);

                if (text == null)
                {
                    continue;
                }

                Grid g = new Grid();

                Border b = new Border()
                {
                    BorderBrush = App.Current.Resources["AccentColor"] as Brush,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(4)
                };

                TextBlock tb = new TextBlock();
                formatter.Markup(tb, text);

                Label l = new Label()
                {
                    Content = tb
                };

                Image i = new Image()
                {
                    Width = 26,
                    Height = 26,
                    Margin = new Thickness(5)
                };

                g.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                g.ColumnDefinitions.Add(new ColumnDefinition());

                g.Children.Add(i);
                g.Children.Add(l);
                Grid.SetColumn(l, 1);

                if (r is IUIL_Icon uIL && uIL.UpdateIcon(out var img))
                {
                    i.Visibility = Visibility.Visible;
                    i.Source = img;
                }
                else
                {
                    i.Visibility = Visibility.Collapsed;
                }

                b.Child = g;

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

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public enum EMode
        {
            BEGIN_QUEST,
            END_QUEST,
            PREVIEW
        }
    }
}
