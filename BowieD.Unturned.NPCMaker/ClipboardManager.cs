using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.NPC.Rewards;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using static BowieD.Unturned.NPCMaker.Controls.Universal_ItemList;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker
{
    public static class ClipboardManager
    {
        public const string
            ConditionFormat = "NPCMaker.Condition",
            RewardFormat = "NPCMaker.Reward",
            CharacterFormat = "NPCMaker.Character",
            DialogueFormat = "NPCMaker.Dialogue",
            VendorFormat = "NPCMaker.Vendor",
            VendorItemFormat = "NPCMaker.Vendor.Item",
            QuestFormat = "NPCMaker.Quest";
        public static void SetObject(ReturnType returnType, object obj)
        {
            SetObject(GetFormat(returnType), obj);
        }
        public static void SetObject<T>(string format, T obj)
        {
            try
            {
                var serializable = obj.GetType().GetCustomAttribute(typeof(System.SerializableAttribute));
                if (serializable != null)
                {
                    Clipboard.SetDataObject(new DataObject(format, obj));
                    App.Logger.Log($"[CLIPBOARD] Set data of format '{format}'", Logging.ELogLevel.DEBUG);
                }
            }
            catch { }
        }
        public static bool TryGetObject(string format, out object obj)
        {
            if (Clipboard.ContainsData(format))
            {
                obj = Clipboard.GetData(format);
                return obj != null;
            }
            else if (Clipboard.ContainsText())
            {
                try
                {
                    string data = Clipboard.GetText();
                    XmlReaderSettings settings = new XmlReaderSettings()
                    {
                        NameTable = new NameTable()
                    };
                    XmlNamespaceManager xmlns = new XmlNamespaceManager(settings.NameTable);
                    xmlns.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    XmlParserContext context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
                    using (StringReader sr = new StringReader(data))
                    using (XmlReader reader = XmlReader.Create(sr, settings, context))
                    {
                        XmlSerializer serializer = CreateDeserializer(GetTypeFromFormat(format));
                        obj = serializer.Deserialize(reader);
                        return true;
                    }
                }
                catch
                {
                    obj = default;
                    return false;
                }
            }
            else
            {
                obj = default;
                return false;
            }
        }
        public static bool TryGetObject(ReturnType returnType, out object obj)
        {
            return TryGetObject(GetFormat(returnType), out obj);
        }

        public static string GetFormat(ReturnType returnType)
        {
            switch (returnType)
            {
                case ReturnType.Character: return CharacterFormat;
                case ReturnType.Condition: return ConditionFormat;
                case ReturnType.Dialogue: return DialogueFormat;
                case ReturnType.Quest: return QuestFormat;
                case ReturnType.Reward: return RewardFormat;
                case ReturnType.Vendor: return VendorFormat;
                case ReturnType.VendorItem: return VendorItemFormat;
                default: return null;
            }
        }
        public static Type GetTypeFromFormat(string format)
        {
            switch (format)
            {
                case CharacterFormat: return typeof(NPCCharacter);
                case ConditionFormat: return typeof(Condition);
                case DialogueFormat: return typeof(NPCDialogue);
                case QuestFormat: return typeof(NPCQuest);
                case RewardFormat: return typeof(Reward);
                case VendorFormat: return typeof(NPCVendor);
                case VendorItemFormat: return typeof(VendorItem);
                default: return null;
            }
        }

        private static XmlSerializer CreateDeserializer(Type type)
        {
            return new XmlSerializer(type);
        }
    }
}
