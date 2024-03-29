﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Configuration;
using System;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCDialogue : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID, IAXData
    {
        public NPCDialogue()
        {
            Messages = new LimitedList<NPCMessage>(byte.MaxValue);
            Responses = new LimitedList<NPCResponse>(byte.MaxValue);
            GUID = Guid.NewGuid().ToString("N");
            Comment = "";
        }

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

        private ushort _id;
        [XmlAttribute("id")]
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

        private LimitedList<NPCMessage> _messages;
        [XmlArray("messages")]
        public LimitedList<NPCMessage> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
                if (!AppConfig.Instance.useCommentsInsteadOfData)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }

        private LimitedList<NPCResponse> _responses;
        [XmlArray("responses")]
        public LimitedList<NPCResponse> Responses
        {
            get => _responses;
            set
            {
                _responses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Responses)));
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void Load(XmlNode node, int version)
        {
            GUID = node.Attributes["guid"].Value;
            Comment = node.Attributes["comment"].Value;
            ID = node.Attributes["id"].ToUInt16();

            Messages = new LimitedList<NPCMessage>(node["messages"].ParseAXDataCollection<NPCMessage>(version), byte.MaxValue);
            Responses = new LimitedList<NPCResponse>(node["responses"].ParseAXDataCollection<NPCResponse>(version), byte.MaxValue);
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(GUID);
            document.CreateAttributeC("comment", node).WriteString(Comment);
            document.CreateAttributeC("id", node).WriteUInt16(ID);

            document.CreateNodeC("messages", node).WriteAXDataCollection(document, "NPCMessage", Messages);
            document.CreateNodeC("responses", node).WriteAXDataCollection(document, "NPCResponse", Responses);
        }

        public string UIText
        {
            get
            {
                if (AppConfig.Instance.useCommentsInsteadOfData)
                {
                    if (string.IsNullOrEmpty(Comment))
                    {
                        return $"[{ID}]";
                    }
                    return TextUtil.Shortify($"[{ID}] - {Comment}", 24);
                }
                else
                {
                    return $"[{ID}] {ContentPreview}";
                }
            }
        }

        public string ContentPreview
        {
            get
            {
                if (Messages == null || Messages.Count < 1 || Messages[0].pages.Count < 1)
                {
                    return string.Empty;
                }
                else
                {
                    string t = Messages[0].pages[0];
                    if (!string.IsNullOrEmpty(t))
                    {
                        return TextUtil.Shortify($"{t}", 24);
                    }

                    return string.Empty;
                }
            }
        }
        public string FullText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var msg in Messages)
                {
                    foreach (var page in msg.pages)
                    {
                        sb.AppendLine(page);
                    }
                }

                foreach (var r in Responses)
                {
                    sb.AppendLine(r.mainText);
                }

                return sb.ToString();
            }
        }
    }
}
