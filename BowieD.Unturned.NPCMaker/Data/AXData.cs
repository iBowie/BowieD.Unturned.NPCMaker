using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.Data
{
    public abstract class AXData<T> : IData<T> where T : IAXData, new()
    {
        protected AXData() { }

        public T data;

        public event DataLoaded<T> OnDataLoaded;
        public event DataSaved<T> OnDataSaved;

        public abstract string FileName { get; }

        public abstract string RootNodeName { get; }
        public virtual string SaveDataNodeName => "SAVEDATA_VERSION";
        public abstract int CurrentSaveDataVersion { get; }

        public bool Save()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", string.Empty));

                XmlNode root = doc.CreateNodeC(RootNodeName, doc);

                var xsiA = doc.CreateAttribute("xmlns:xsi", "http://www.w3.org/2000/xmlns/");
                xsiA.Value = "http://www.w3.org/2001/XMLSchema-instance";
                doc.DocumentElement.Attributes.Append(xsiA);

                var xsdA = doc.CreateAttribute("xmlns:xsd", "http://www.w3.org/2000/xmlns/");
                xsdA.Value = "http://www.w3.org/2001/XMLSchema";
                doc.DocumentElement.Attributes.Append(xsdA);

                doc.CreateNodeC(SaveDataNodeName, root).WriteInt32(CurrentSaveDataVersion);

                data.Save(doc, root);

                using (XmlTextWriter xtw = new XmlTextWriter(FileName, Encoding.UTF8))
                {
                    xtw.Formatting = Formatting.None;

                    doc.Save(xtw);
                }

                return true;
            }
            catch (Exception ex)
            {
                App.Logger.LogException($"[AXDATA] - Could not save to {FileName}", ex: ex);

                return false;
            }
        }

        protected abstract void GetRootAndVersion(XmlDocument document, out XmlNode root, out int version);

        public bool Load(T defaultValue)
        {
            App.Logger.Log($"[AXDATA] - Loading {FileName}");

            if (File.Exists(FileName))
            {
                App.Logger.Log($"[AXDATA] - Parsing XML...");

                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);

                try
                {
                    data = new T();

                    GetRootAndVersion(doc, out var root, out var version);

                    data.Load(root, version);

                    App.Logger.Log($"[AXDATA] - Loaded");
                    return true;
                }
                catch (Exception ex)
                {
                    App.Logger.LogException($"[AXDATA] - Could not load from {FileName}", ex: ex);

                    data = defaultValue;

                    return false;
                }
            }
            else
            {
                App.Logger.Log($"[AXDATA] - {FileName} does not exist. Ignoring...");

                data = defaultValue;

                return false;
            }
        }
    }
}
