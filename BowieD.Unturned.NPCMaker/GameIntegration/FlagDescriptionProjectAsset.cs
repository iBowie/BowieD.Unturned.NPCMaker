using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using System.Windows;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class FlagDescriptionProjectAsset : ProjectAsset, IEditable, ICreatable, IDeletable
    {
        public FlagDescriptionProjectAsset() : base("Unnamed", 0, "PROJ_FLAG")
        {

        }
        public void Edit(Window calledFrom)
        {
            MultiFieldInputView_Dialog mfiv = new MultiFieldInputView_Dialog(new string[2]
            {
                name,
                id.ToString()
            })
            {
                Owner = calledFrom,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (mfiv.ShowDialog(new string[2] { LocalizationManager.Current.Simulation["Flags"]["Flag_Name"], LocalizationManager.Current.Simulation["Flags"]["Flag_ID"] }, "") == true)
            {
                string[] values = mfiv.Values;
                if (ushort.TryParse(values[1], out ushort flagID))
                {
                    this.name = values[0];
                    this.id = flagID;
                }
            }
        }

        public void OnCreate()
        {
            MainWindow.CurrentProject.data.flags.Add(this);
        }

        public void OnDelete()
        {
            MainWindow.CurrentProject.data.flags.Remove(this);
        }
    }
}
