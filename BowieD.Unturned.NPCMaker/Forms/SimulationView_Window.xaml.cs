using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for SimulationView_Window.xaml
    /// </summary>
    public partial class SimulationView_Window : Window
    {
        public SimulationView_Window(DialogueView_Window dialogueView, Simulation simulation)
        {
            InitializeComponent();

            this.Simulation = simulation;
            this.DialogueView = dialogueView;
            DataContext = simulation;

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
    }
}
