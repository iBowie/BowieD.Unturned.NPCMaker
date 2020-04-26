using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogueView.Display();
        }
    }
}
