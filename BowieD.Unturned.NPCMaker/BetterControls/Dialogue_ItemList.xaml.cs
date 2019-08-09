using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.BetterControls
{
    /// <summary>
    /// Логика взаимодействия для Dialogue_ItemList.xaml
    /// </summary>
    public partial class Dialogue_ItemList : UserControl
    {
        public Dialogue_ItemList(NPC.NPCDialogue dialogue)
        {
            InitializeComponent();
            Item = dialogue;
        }

        public NPC.NPCDialogue Item { get; set; }
    }
}
