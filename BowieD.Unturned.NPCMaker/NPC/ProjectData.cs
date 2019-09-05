using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.Logging;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class ProjectData : XmlData<NPCProject>
    {
        public ProjectData()
        {
            file = "";
            data = new NPCProject();
        }
        public string file;
        public override string FileName => file;
        public bool isSaved = false;
        /// <summary>
        /// True - Yes / No prompt needed
        /// False - No
        /// Null - Cancel
        /// </summary>
        /// <returns></returns>
        public bool? SavePrompt()
        {
            if (isSaved)
                return true;
            var result = MessageBox.Show(LocalizationManager.Current.Interface["Project_UnsavedChanges_Text"], LocalizationManager.Current.Interface["Project_UnsavedChanges_Title"], MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Save();
                return true;
            }
            else if (result == MessageBoxResult.No)
                return false;
            else
                return null;
        }
        public override bool Save()
        {
            if (file == "")
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = "Unnamed",
                    DefaultExt = ".npcproj",
                    Filter = "NPC Project (.npcproj)|*.npcproj"
                };
                if (sfd.ShowDialog() == true)
                {
                    file = sfd.FileName;
                    isSaved = base.Save();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                isSaved = base.Save();
            }
            return isSaved;
        }
        public override bool Load(NPCProject defaultValue)
        {
            App.Logger.LogInfo($"[XDATA] - Loading {FileName}!");
            if (File.Exists(FileName))
            {
                App.Logger.LogInfo($"[XDATA] - Converting from XML...");
                using (FileStream fs = new FileStream(FileName, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    if (_serializer.CanDeserialize(reader))
                    {
                        data = (NPCProject)_serializer.Deserialize(reader);
                        App.Logger.LogInfo($"[XDATA] - Loaded");
                        return true;
                    }
                    else
                    {
                        App.Logger.LogInfo($"[XDATA] - Could not load {FileName}. Ignoring...");
                        return false;
                    }
                }
            }
            else
            {
                App.Logger.LogInfo($"[XDATA] - {FileName} does not exist. Ignoring...");
                return false;
            }
        }
    }
}
