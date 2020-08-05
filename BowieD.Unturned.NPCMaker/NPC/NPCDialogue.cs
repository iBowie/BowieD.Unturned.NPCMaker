using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    [System.Serializable]
    public class NPCDialogue : IHasUIText, INotifyPropertyChanged, IHasUniqueGUID
    {
        public NPCDialogue()
        {
            Messages = new List<NPCMessage>();
            Responses = new List<NPCResponse>();
            GUID = Guid.NewGuid().ToString("N");
            Comment = "";
        }

        [XmlAttribute("guid")]
        public string GUID { get; set; }
        [XmlAttribute("comment")]
        public string Comment { get; set; }

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

        private List<NPCMessage> _messages;
        [XmlArray("messages")]
        public List<NPCMessage> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Messages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UIText)));
            }
        }

        private List<NPCResponse> _responses;
        [XmlArray("responses")]
        public List<NPCResponse> Responses
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

        public List<NPCResponse> GetVisibleResponses(NPCMessage message)
        {
            int messageIndex = Messages.IndexOf(message);
            if (messageIndex == -1)
            {
                return null;
            }

            return Responses.Where(d => d.VisibleInAll || d.visibleIn[messageIndex] == 1).ToList();
        }
        public string UIText
        {
            get
            {
                if (Messages == null || Messages.Count < 1 || Messages[0].pages.Count < 1)
                {
                    return $"[{ID}]";
                }
                else
                {
                    string t = Messages[0].pages[0];
                    if (!string.IsNullOrEmpty(t))
                    {
                        return TextUtil.Shortify($"[{ID}] - {t}", 24);
                    }

                    return $"[{ID}]";
                }
            }
        }
    }
}
