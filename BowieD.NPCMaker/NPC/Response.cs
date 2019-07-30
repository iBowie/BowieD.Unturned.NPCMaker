using System.Collections.Generic;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Response
    {
        public Response()
        {
            conditions = new List<Condition.Condition>();
            rewards = new List<Reward.Reward>();
            visiblePages = new HashSet<int>();
        }
        public string text;
        public ushort dialogueId;
        public ushort vendorId;
        public ushort questId;
        public List<Condition.Condition> conditions;
        public List<Reward.Reward> rewards;
        public HashSet<int> visiblePages;
    }
}
