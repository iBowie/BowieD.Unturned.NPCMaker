using BowieD.Unturned.NPCMaker.Coloring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Condition = BowieD.Unturned.NPCMaker.NPC.Conditions.Condition;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCCharacter : IHasUIText, INotifyPropertyChanged
    {
        public NPCCharacter()
        {
            guid = Guid.NewGuid().ToString("N");
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

        [XmlAttribute]
        public string guid;
        public List<Condition> visibilityConditions;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public string UIText => $"[{ID}] {EditorName} - {DisplayName}";
    }
}
