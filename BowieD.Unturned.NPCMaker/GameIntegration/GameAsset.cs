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
        public GameAsset(string name, ushort id, Guid guid, string type, EGameAssetOrigin origin)
        {
            this.name = name;
            this.id = id;
            this.guid = guid;
            this.type = type;
            this.origin = origin;
        }

        public string name;
        public ushort id;
        public Guid guid;
        public string type;
        public EGameAssetOrigin origin;
    }
    public enum EGameAssetOrigin
    {
        Unturned,
        Workshop,
        Project
    }
    public class GameItemAsset : GameAsset
    {
        public static readonly Uri DefaultImagePath = new Uri("pack://application:,,,/Resources/Icons/unknown.png");

        public GameItemAsset(DataReader data, string dirName, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
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
        public GameVehicleAsset(DataReader data, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {

        }
    }
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
    }
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
    public class GameDialogueAsset : GameAsset
    {
        public GameDialogueAsset(NPCDialogue dialogue, EGameAssetOrigin origin) : base($"Asset_{dialogue.ID}", dialogue.ID, Guid.Parse(dialogue.GUID), "Dialogue", origin)
        {
            this.dialogue = dialogue;
        }
        public GameDialogueAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            dialogue = new Parsing.ParseTool(data, local).ParseDialogue();
        }

        public NPCDialogue dialogue;
    }
    public class GameVendorAsset : GameAsset
    {
        public GameVendorAsset(NPCVendor vendor, EGameAssetOrigin origin) : base(vendor.Title, vendor.ID, Guid.Parse(vendor.GUID), "Vendor", origin)
        {
            this.vendor = vendor;
        }
        public GameVendorAsset(DataReader data, DataReader local, string name, ushort id, Guid guid, string type, EGameAssetOrigin origin) : base(name, id, guid, type, origin)
        {
            vendor = new Parsing.ParseTool(data, local).ParseVendor();
        }

        public NPCVendor vendor;
    }
}
