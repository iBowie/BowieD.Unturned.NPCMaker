using BowieD.Unturned.NPCMaker.Common.Utility;
using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.NPC;
using BowieD.Unturned.NPCMaker.Parsing;
using System;
using System.IO;

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
        public static readonly Uri DefaultImagePath = new Uri("pack://application:,,,/Resources/Icons/unknown.png");

        public GameItemAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type) : base(name, id, guid, type)
        {
            this.dirName = dirName;
        }

        private string dirName;

        public Uri ImagePath
        {
            get
            {
                var fallback = DefaultImagePath;

                var uDir = AppConfig.Instance.unturnedDir;
                if (string.IsNullOrEmpty(uDir) || !Directory.Exists(uDir) || !PathUtility.IsUnturnedPath(uDir))
                {
                    return fallback;
                }
                else
                {
                    string fName = Path.Combine(uDir, "Extras", "Icons", $"{dirName}_{id}.png");

                    if (File.Exists(fName))
                    {
                        return new Uri(Path.GetFullPath(fName));
                    }
                    else
                    {
                        return fallback;
                    }
                }
            }
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
