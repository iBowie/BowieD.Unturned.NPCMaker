using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameAsset
    {
        public GameAsset(string name, ushort id, Guid guid, string type)
        {
            this.name = name;
            this.id = id;
            this.guid = guid;
            this.type = type;
        }

        public string name;
        public ushort id;
        public Guid guid;
        public string type;
    }
    public class GameItemAsset : GameAsset
    {
        public GameItemAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {

        }
    }
    public class GameVehicleAsset : GameAsset
    {
        public GameVehicleAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {

        }
    }
    public class GameNPCAsset : GameAsset
    {
        public GameNPCAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {
            character = new Parsing.ParseTool(data, local).ParseCharacter();
        }

        public NPCCharacter character;
    }
    public class GameQuestAsset : GameAsset
    {
        public GameQuestAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {
            quest = new Parsing.ParseTool(data, local).ParseQuest();
        }

        public NPCQuest quest;
    }
    public class GameDialogueAsset : GameAsset
    {
        public GameDialogueAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {
            dialogue = new Parsing.ParseTool(data, local).ParseDialogue();
        }

        public NPCDialogue dialogue;
    }
    public class GameVendorAsset : GameAsset
    {
        public GameVendorAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {
            vendor = new Parsing.ParseTool(data, local).ParseVendor();
        }

        public NPCVendor vendor;
    }
}
