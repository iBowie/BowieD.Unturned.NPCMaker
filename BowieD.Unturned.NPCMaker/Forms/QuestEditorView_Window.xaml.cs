using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System.Collections.Generic;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for QuestEditorView_Window.xaml
    /// </summary>
    public partial class QuestEditorView_Window : Window
    {
        public QuestEditorView_Window(Simulation simulation)
        {
            InitializeComponent();

            DataContext = this;

            Simulation = simulation;
        }

        public Simulation Simulation { get; }
        public HashSet<ushort> Quests => Simulation.Quests;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        ask:
            OneFieldInputView_Dialog ofiv = new OneFieldInputView_Dialog();
            if (ofiv.ShowDialog(LocalizationManager.Current.Simulation["Quests"]["Quest_ID"], LocalizationManager.Current.Simulation["Quests"]["Quest_Add"]) == true)
            {
                if (ushort.TryParse(ofiv.Value, out ushort flagID))
                {
                    if (Quests.Add(flagID))
                    {
                        list.ItemsSource = Quests;
                    }
                }
                else
                {
                    goto ask;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        ask:
            OneFieldInputView_Dialog ofiv = new OneFieldInputView_Dialog();
            if (ofiv.ShowDialog(LocalizationManager.Current.Simulation["Quests"]["Quest_ID"], LocalizationManager.Current.Simulation["Quests"]["Quest_Remove"]) == true)
            {
                if (ushort.TryParse(ofiv.Value, out ushort flagID))
                {
                    if (Quests.Remove(flagID))
                    {
                        list.ItemsSource = Quests;
                    }
                }
                else
                {
                    goto ask;
                }
            }
        }
    }
}
