using BowieD.Unturned.NPCMaker.Coloring;
using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCCharacter : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID
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

        [XmlAttribute("guid")]
        public string GUID { get; set; }
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
                            sb.Append(" - ");
                        sb.Append(DisplayName);
                    }
                    return TextUtil.Shortify(sb.ToString(), 24);
                }
            }
        }
    }
}
