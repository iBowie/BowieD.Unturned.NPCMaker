using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameQuestAsset : GameAsset
    {
        public GameQuestAsset(NPCQuest quest, EGameAssetOrigin origin) : base(quest.Title, quest.ID, Guid.Parse(quest.GUID), "Quest", origin)
        {
            this.quest = quest;
        }
        public GameQuestAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            quest = new Parsing.ParseTool(data, local).ParseQuest();
        }

        public NPCQuest quest;
    }
}
