using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for Character_PoseEditor.xaml
    /// </summary>
    public partial class Character_PoseEditor : Window
    {
        public Character_PoseEditor(NPCCharacter character)
        {
            InitializeComponent();

            DataContext = this;

            Character = character;
        }

        public NPCCharacter Character { get; }

        public NPC_Pose Pose
        {
            get
            {
                return Character.pose;
            }
            set
            {
                Character.pose = value;
            }
        }
        public float Head_Offset
        {
            get
            {
                return Character.poseHeadOffset;
            }
            set
            {
                Character.poseHeadOffset = value;
            }
        }
        public float Lean
        {
            get
            {
                return Character.poseLean;
            }
            set
            {
                Character.poseLean = value;
            }
        }
        public float Pitch
        {
            get
            {
                return Character.posePitch;
            }
            set
            {
                Character.posePitch = value;
            }
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            Pose = NPC_Pose.Stand;
            Head_Offset = 0f;
            Lean = 0f;
            Pitch = 90f;
            Close();
        }

        private void Pitch_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pitch_visible.Text = $"{e.NewValue:0.##}°";
        }

        private void Lean_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double value = e.NewValue * 45f;
            lean_visible.Text = $"{value:0.##}°";
        }
    }
}
