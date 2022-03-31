using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using MahApps.Metro.Controls;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for SimulationView_Window.xaml
    /// </summary>
    public partial class SimulationView_Window : MetroWindow
    {
        public SimulationView_Window(DialogueView_Window dialogueView, Simulation simulation)
        {
            InitializeComponent();

            Simulation = simulation;
            DialogueView = dialogueView;
            DataContext = simulation;

            if (dialogueView == null)
            {
                reloadDialogueButton.IsEnabled = false;
                reloadDialogueButton.Visibility = Visibility.Collapsed;
            }

            gameTimeSlider.ValueChanged += (sender, e) =>
            {
                gameTimeSlider.ToolTip = ConditionTimeOfDay.SecondToTime((int)gameTimeSlider.Value);
            };
        }

        public DialogueView_Window DialogueView { get; }
        public Simulation Simulation { get; }

        private void ReloadDialogue_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogueView.Display();
        }

        private void FlagEditor_Button_Click(object sender, RoutedEventArgs e)
        {
            FlagEditorView_Window fevw = new FlagEditorView_Window(Simulation);
            fevw.Owner = this;
            fevw.ShowDialog();
        }
        private void QuestEditor_Button_Click(object sender, RoutedEventArgs e)
        {
            QuestEditorView_Window qevw = new QuestEditorView_Window(Simulation);
            qevw.Owner = this;
            qevw.ShowDialog();
        }
        private void InventoryEditor_Button_Click(object sender, RoutedEventArgs e)
        {
            InventoryEditorView_Window ievw = new InventoryEditorView_Window(Simulation);
            ievw.Owner = this;
            ievw.ShowDialog();
        }
    }
}
