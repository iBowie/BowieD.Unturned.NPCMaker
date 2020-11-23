using BowieD.Unturned.NPCMaker.Parsing;
using System;

namespace BowieD.Unturned.NPCMaker.GameIntegration
{
    public class GameAsset
    {
        public GameAsset(DataReader data, string name, ushort id, Guid guid, string type)
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
        public GameItemAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
    public class GameVehicleAsset : GameAsset
    {
        public GameVehicleAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
    public class GameNPCAsset : GameAsset
    {
        public GameNPCAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
    public class GameQuestAsset : GameAsset
    {
        public GameQuestAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
    public class GameDialogueAsset : GameAsset
    {
        public GameDialogueAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
    public class GameVendorAsset : GameAsset
    {
        public GameVendorAsset(DataReader data, string name, ushort id, Guid guid, string type) : base(data, name, id, guid, type)
        {

        }
    }
}
