using System.Collections.Generic;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCMessage
    {
        public NPCMessage()
        {
            pages = new List<string>();
            conditions = new Condition[0];
        }

        public List<string> pages;
        [XmlIgnore]
        public int PagesAmount => pages == null ? 0 : pages.Count;
        public NPC.Condition[] conditions;
    }
}
