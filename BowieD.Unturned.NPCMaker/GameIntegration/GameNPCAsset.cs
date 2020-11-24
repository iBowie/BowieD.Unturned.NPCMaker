using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameNPCAsset : GameObjectAsset
    {
        public GameNPCAsset(NPCCharacter character, EGameAssetOrigin origin) : base(Guid.Parse(character.GUID), origin)
        {
            this.character = character;
        }
        public GameNPCAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(data, name, id, guid, type, origin)
        {
            character = new Parsing.ParseTool(data, local).ParseCharacter();
        }

        public NPCCharacter character;
    }
}
