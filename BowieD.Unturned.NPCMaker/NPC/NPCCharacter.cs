using BowieD.Unturned.NPCMaker.Coloring;
using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCCharacter : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID, IAXData
    {
        public NPCCharacter()
        {
            GUID = Guid.NewGuid().ToString("N");
            Comment = "";
            EditorName = "";
            DisplayName = "";
            ID = 0;
            startDialogueId = 0;
            visibilityConditions = new List<Condition>();
            face = 0;
            beard = 0;
            haircut = 0;
            hairColor = new Color(0, 0, 0);
            skinColor = new Color(0, 0, 0);
            clothing = new NPCClothing();
            halloweenClothing = new NPCClothing();
            christmasClothing = new NPCClothing();
            pose = NPC_Pose.Stand;
            leftHanded = false;
            equipPrimary = 0;
            equipSecondary = 0;
            equipTertiary = 0;
            equipped = Equip_Type.None;
            poseLean = 0f;
            posePitch = 90f;
            poseHeadOffset = 0f;
        }
        private string _editorName;
        [XmlElement("editorName")]
        public string EditorName
        {
            get => _editorName;
            set
            {
                _editorName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditorName)));
                if (!AppConfig.Instance.useCommentsInsteadOfData)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        private string _displayName;
        [XmlElement("displayName")]
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                if (!AppConfig.Instance.useCommentsInsteadOfData)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        private ushort _id;
        [XmlElement("id")]
        public ushort ID
        {
            get => _id;
            set
            {
                _id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        public ushort startDialogueId;
        public byte face;
        public byte beard;
        public byte haircut;
        public Color hairColor;
        public Color skinColor;
        public NPCClothing clothing;
        public NPCClothing christmasClothing;
        public NPCClothing halloweenClothing;
        public NPC_Pose pose;
        public bool leftHanded;
        public ushort equipPrimary;
        public ushort equipSecondary;
        public ushort equipTertiary;
        public Equip_Type equipped;
        public float poseLean, posePitch, poseHeadOffset;

        private string _guid;
        [XmlAttribute("guid")]
        public string GUID 
        {
            get => _guid;
            set
            {
                _guid = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GUID)));
            }
        }
        private string _comment;
        [XmlAttribute("comment")]
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));
                if (AppConfig.Instance.useCommentsInsteadOfData)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }
        public List<Condition> visibilityConditions;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public string UIText
        {
            get
            {
                if (AppConfig.Instance.useCommentsInsteadOfData)
                {
                    if (string.IsNullOrEmpty(Comment))
                        return $"[{ID}]";
                    return TextUtil.Shortify($"[{ID}] - {Comment}", 24);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"[{ID}]");
                    bool flag = false;
                    if (!string.IsNullOrEmpty(EditorName))
                    {
                        sb.Append(" ");
                        sb.Append(EditorName);
                        flag = true;
                    }
                    if (!string.IsNullOrEmpty(DisplayName))
                    {
                        if (flag)
                            sb.Append(" -");
                        sb.Append(" ");
                        sb.Append(DisplayName);
                    }
                    return TextUtil.Shortify(sb.ToString(), 24);
                }
            }
        }

        public void Load(XmlNode node, int version)
        {
            GUID = node.Attributes["guid"].ToText();
            Comment = node.Attributes["comment"].ToText();

            EditorName = node["editorName"].ToText();
            DisplayName = node["displayName"].ToText();
            ID = node["id"].ToUInt16();
            startDialogueId = node["startDialogueId"].ToUInt16();
            face = node["face"].ToByte();
            beard = node["beard"].ToByte();
            haircut = node["haircut"].ToByte();
            hairColor = node["hairColor"].ToColor(version);
            skinColor = node["skinColor"].ToColor(version);

            clothing.Load(node["clothing"], version);
            christmasClothing.Load(node["christmasClothing"], version);
            halloweenClothing.Load(node["halloweenClothing"], version);

            pose = node["pose"].ToEnum<NPC_Pose>();

            leftHanded = node["leftHanded"].ToBoolean();

            equipPrimary = node["equipPrimary"].ToUInt16();
            equipSecondary = node["equipSecondary"].ToUInt16();
            equipTertiary = node["equipTertiary"].ToUInt16();

            equipped = node["equipped"].ToEnum<Equip_Type>();

            poseLean = node["poseLean"].ToSingle();
            posePitch = node["posePitch"].ToSingle();
            poseHeadOffset = node["poseHeadOffset"].ToSingle();

            visibilityConditions = node["visibilityConditions"].ParseAXDataCollection<Condition>(version).ToList();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(GUID);
            document.CreateAttributeC("comment", node).WriteString(Comment);

            document.CreateNodeC("editorName", node).WriteString(EditorName);
            document.CreateNodeC("displayName", node).WriteString(DisplayName);
            document.CreateNodeC("id", node).WriteUInt16(ID);
            document.CreateNodeC("startDialogueId", node).WriteUInt16(startDialogueId);
            document.CreateNodeC("face", node).WriteByte(face);
            document.CreateNodeC("beard", node).WriteByte(beard);
            document.CreateNodeC("haircut", node).WriteByte(haircut);
            document.CreateNodeC("hairColor", node).WriteColor(hairColor);
            document.CreateNodeC("skinColor", node).WriteColor(skinColor);

            clothing.Save(document, document.CreateNodeC("clothing", node));
            christmasClothing.Save(document, document.CreateNodeC("christmasClothing", node));
            halloweenClothing.Save(document, document.CreateNodeC("halloweenClothing", node));

            document.CreateNodeC("pose", node).WriteEnum(pose);

            document.CreateNodeC("leftHanded", node).WriteBoolean(leftHanded);

            document.CreateNodeC("equipPrimary", node).WriteUInt16(equipPrimary);
            document.CreateNodeC("equipSecondary", node).WriteUInt16(equipSecondary);
            document.CreateNodeC("equipTertiary", node).WriteUInt16(equipTertiary);

            document.CreateNodeC("equipped", node).WriteEnum(equipped);

            document.CreateNodeC("poseLean", node).WriteSingle(poseLean);
            document.CreateNodeC("posePitch", node).WriteSingle(posePitch);
            document.CreateNodeC("poseHeadOffset", node).WriteSingle(poseHeadOffset);

            document.CreateNodeC("visibilityConditions", node).WriteAXDataCollection(document, "Condition", visibilityConditions);
        }
    }
}
