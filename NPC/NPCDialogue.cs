using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCDialogue
    {
        public NPCDialogue()
        {
            messages = new List<NPCMessage>();
            responses = new List<NPCResponse>();
            guid = Guid.NewGuid().ToString("N");
            comment = "";
        }

        [XmlAttribute]
        public string guid;
        [XmlAttribute]
        public string comment;

        [XmlAttribute]
        public ushort id;
        public List<NPCMessage> messages;
        public int MessagesAmount => messages == null ? 0 : messages.Count;
        [XmlIgnore]
        public int ResponsesAmount => responses == null ? 0 : responses.Count;
        public List<NPCResponse> responses;

        public override string ToString()
        {
            return $"[{id}]";
        }
    }
}
