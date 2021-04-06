using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Forms;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC;
using System.Windows;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class FlagDescriptionProjectAsset : ProjectAsset, IEditable, ICreatable, IDeletable, IAXData
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

        public void Load(XmlNode node, int version)
        {
            name = node["name"].InnerText;
            id = node["id"].ToUInt16();
        }

        public void OnCreate()
        {
            MainWindow.CurrentProject.data.flags.Add(this);
        }

        public void OnDelete()
        {
            MainWindow.CurrentProject.data.flags.Remove(this);
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("name", node).WriteString(name);
            document.CreateNodeC("id", node).WriteUInt16(id);
        }
    }
}
