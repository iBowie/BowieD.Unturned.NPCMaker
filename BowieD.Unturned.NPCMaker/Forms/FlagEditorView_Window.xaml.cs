using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Interaction logic for FlagEditorView_Window.xaml
    /// </summary>
    public partial class FlagEditorView_Window : MetroWindow
    {
        public FlagEditorView_Window(Simulation simulation)
        {
            InitializeComponent();

            DataContext = this;

            Simulation = simulation;

            foreach (var f in Flags)
                list.Items.Add(f);
        }

        public Simulation Simulation { get; }
        public Dictionary<ushort, short> Flags => Simulation.Flags;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        ask:
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog();
            if (mfiv.ShowDialog(new string[2] { LocalizationManager.Current.Simulation["Flags"]["Flag_ID"], LocalizationManager.Current.Simulation["Flags"]["Flag_Value"] }, LocalizationManager.Current.Simulation["Flags"]["Flag_Set"]) == true)
            {
                string[] values = mfiv.Values;
                if (ushort.TryParse(values[0], out ushort flagID) && short.TryParse(values[1], out short flagValue))
                {
                    Flags[flagID] = flagValue;
                    list.Items.Add(new KeyValuePair<ushort, short>(flagID, flagValue));
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
            if (ofiv.ShowDialog(LocalizationManager.Current.Simulation["Flags"]["Flag_ID"], LocalizationManager.Current.Simulation["Flags"]["Flag_Remove"]) == true)
            {
                if (ushort.TryParse(ofiv.Value, out ushort flagID))
                {
                    if (Flags.Remove(flagID))
                    {
                        list.Items.Clear();

                        foreach (var f in Flags)
                            list.Items.Add(f);
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
