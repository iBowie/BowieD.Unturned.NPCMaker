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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
