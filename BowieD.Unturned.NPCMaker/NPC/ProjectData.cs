using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Data;
using BowieD.Unturned.NPCMaker.Localization;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class ProjectData : XmlData<NPCProject>
    {
        public static ProjectData CurrentProject { get; } = new ProjectData();

        public ProjectData()
        {
            file = "";
            data = new NPCProject();
        }
        public string file;
        public override string FileName => file;
        public bool isSaved = false;
        public bool hasLoadedAtLeastOnce = false;

        public new event DataLoaded<NPCProject> OnDataLoaded;

        /// <summary>
        /// True - Yes / No prompt needed
        /// False - No
        /// Null - Cancel
        /// </summary>
        /// <returns></returns>
        public bool? SavePrompt()
        {
            if (isSaved)
            {
                return true;
            }

            MessageBoxResult result = MessageBox.Show(LocalizationManager.Current.Interface["Project_UnsavedChanges_Text"], LocalizationManager.Current.Interface["Project_UnsavedChanges_Title"], MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                Save();
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return null;
            }
        }
        public override bool Save()
        {
            if (file == "")
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = "Unnamed",
                    DefaultExt = ".npcproj",
                    Filter = $"{LocalizationManager.Current.General["Project_SaveFilter"]}|*.npcproj"
                };
                if (sfd.ShowDialog() == true)
                {
                    file = sfd.FileName;

                    DoSave();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                DoSave();
            }
            return isSaved;
        }
        internal void DoSave()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", string.Empty));

                XmlNode root = doc.CreateNodeC("NPCProject", doc);

                var xsiA = doc.CreateAttribute("xmlns:xsi", "http://www.w3.org/2000/xmlns/");
                xsiA.Value = "http://www.w3.org/2001/XMLSchema-instance";
                doc.DocumentElement.Attributes.Append(xsiA);

                var xsdA = doc.CreateAttribute("xmlns:xsd", "http://www.w3.org/2000/xmlns/");
                xsdA.Value = "http://www.w3.org/2001/XMLSchema";
                doc.DocumentElement.Attributes.Append(xsdA);

                doc.CreateNodeC("SAVEDATA_VERSION", root).WriteInt32(NPCProject.CURRENT_SAVEDATA_VERSION);

                data.Save(doc, root);

                using (XmlTextWriter xtw = new XmlTextWriter(FileName, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.None;

                    doc.Save(xtw);
                }

                isSaved = true;
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"[AXDATA] - Could not save to {FileName}", ex: ex);
            }
        }
        public override bool Load(NPCProject defaultValue)
        {
            App.Logger.Log($"[AXDATA] - Loading {FileName}");

            if (File.Exists(FileName))
            {
                App.Logger.Log($"[AXDATA] - Parsing XML...");

                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);

                try
                {
                    NPCProject project = new NPCProject();

                    var oldRoot = doc["NPCSave"];

                    if (oldRoot != null)
                    {
                        var root = oldRoot;

                        int ver = -1;

                        project.Load(root, ver);

                        data = project;
                        App.Logger.Log($"[AXDATA] - Loaded legacy project");
                        OnDataLoaded?.Invoke();
                        hasLoadedAtLeastOnce = true;
                        return true;
                    }
                    else
                    {
                        var root = doc["NPCProject"];

                        int ver = root["SAVEDATA_VERSION"].ToInt32();

                        if (ver <= NPCProject.CURRENT_SAVEDATA_VERSION)
                        {
                            project.Load(root, ver);

                            data = project;
                            App.Logger.Log($"[AXDATA] - Loaded");
                            OnDataLoaded?.Invoke();
                            hasLoadedAtLeastOnce = true;
                            return true;
                        }
                        else
                        {
                            App.Logger.Log($"[AXDATA] - Tried to load newer project file");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"[AXDATA] - Could not load project from {FileName}", ex: ex);
                    return false;
                }
            }
            else
            {
                App.Logger.Log($"[AXDATA] - {FileName} does not exist. Ignoring...");
                return false;
            }
        }
    }
}
