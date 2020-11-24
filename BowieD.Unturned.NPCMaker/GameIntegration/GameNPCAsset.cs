using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameNPCAsset : GameAsset
    {
        public GameNPCAsset(NPCCharacter character, EGameAssetOrigin origin) : base(character.EditorName, character.ID, Guid.Parse(character.GUID), "NPC", origin)
        {
            this.character = character;
        }
        public GameNPCAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            character = new Parsing.ParseTool(data, local).ParseCharacter();
        }

        public NPCCharacter character;

        public override EGameAssetCategory Category => EGameAssetCategory.OBJECT;
    }
}
