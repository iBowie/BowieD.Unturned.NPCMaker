using System.Collections.Generic;
namespace BowieD.NPCMaker.NPC
{
    public sealed class Dialogue
    {
        public Dialogue()
        {
            guid = Guid.NewGuid().ToString("N");
            messages = new List<Message>();
            responses = new List<Response>();
        }
        public string guid;
        public ushort id;
        public List<Message> messages;
        public List<Response> responses;
    }
}
