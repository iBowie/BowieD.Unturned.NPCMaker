﻿using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.NPC.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCProject : IAXData
    {
        public const int CURRENT_SAVEDATA_VERSION = 16;
        /*
         * SAVEDATA_VERSION information
         * 
         * -1  - legacy NPC Maker versions
         *  1  - initial new project format
         *  2  - 
         *  3  - added last selected tabs
         *  4  - added currencies
         *  5  - added flag descriptions
         *  6  - added project settings
         *  7  - added dialogue vendors
         *  8  - 
         *  9  - split buying and selling items in to separate collections, added MinRadius to ZombieKills
         *  10 - added rewards to vendor items
         *  11 - added GUID/ID bridge
         *  12 - added spawnpoint reward
         *  13 - added 'B_Value' for flag math reward, added modulo operation type
         *  14 - removed dialogue vendors
         *  15 - added 'Is Full Moon' condition
         *  16 - added 'Is Visible In Vendor Menu' for currency
         */

        public NPCProject()
        {
            guid = Guid.NewGuid().ToString("N");
            SAVEDATA_VERSION = CURRENT_SAVEDATA_VERSION;
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            quests = new List<NPCQuest>();
            currencies = new List<CurrencyAsset>();
            flags = new List<FlagDescriptionProjectAsset>();
            lastCharacter = -1;
            lastDialogue = -1;
            lastVendor = -1;
            lastQuest = -1;
            lastCurrency = -1;
            settings = new NPCProjectSettings();
        }

        [XmlAttribute]
        public string guid;
        public int SAVEDATA_VERSION;
        public List<NPCCharacter> characters;
        public List<NPCDialogue> dialogues;
        public List<NPCVendor> vendors;
        public List<NPCQuest> quests;
        public List<CurrencyAsset> currencies;
        public List<FlagDescriptionProjectAsset> flags;
        public int
            lastCharacter = -1,
            lastDialogue = -1,
            lastVendor = -1,
            lastQuest = -1,
            lastCurrency = -1;
        public NPCProjectSettings settings;

        public void Load(XmlNode node, int version)
        {
            guid = node.Attributes["guid"].Value;

            if (version == -1)
            {
                NPCCharacter mainCharacter = new NPCCharacter()
                {
                    EditorName = node["editorName"].ToText(),
                    DisplayName = node["displayName"].ToText(),
                    ID = node["id"].ToUInt16(),
                    face = node["face"].ToByte(),
                    beard = node["beard"].ToByte(),
                    haircut = node["haircut"].ToByte(),
                    hairColor = node["hairColor"].ToColor(version),
                    skinColor = node["skinColor"].ToColor(version),
                    clothing = new NPCClothing()
                    {
                        Backpack = (GUIDIDBridge)node["backpack"].ToUInt16(),
                        Hat = (GUIDIDBridge)node["hat"].ToUInt16(),
                        Mask = (GUIDIDBridge)node["mask"].ToUInt16(),
                        Shirt = (GUIDIDBridge)node["top"].ToUInt16(),
                        Pants = (GUIDIDBridge)node["bottom"].ToUInt16(),
                        Vest = (GUIDIDBridge)node["vest"].ToUInt16()
                    },
                    startDialogueId = node["startDialogueId"].ToUInt16(),
                    pose = node["pose"].ToEnum<NPC_Pose>(),
                    equipped = node["equipped"].ToEnum<Equip_Type>(),
                    equipPrimary = (GUIDIDBridge)node["equipPrimary"].ToUInt16(),
                    equipSecondary = (GUIDIDBridge)node["equipSecondary"].ToUInt16(),
                    equipTertiary = (GUIDIDBridge)node["equipTertiary"].ToUInt16(),
                    leftHanded = node["leftHanded"].ToBoolean()
                };

                characters = new List<NPCCharacter>()
                {
                    mainCharacter
                };
            }
            else
            {
                characters = node["characters"].ParseAXDataCollection<NPCCharacter>(version).ToList();
            }
            dialogues = node["dialogues"].ParseAXDataCollection<NPCDialogue>(version).ToList();
            vendors = node["vendors"].ParseAXDataCollection<NPCVendor>(version).ToList();
            quests = node["quests"].ParseAXDataCollection<NPCQuest>(version).ToList();

            if (version >= 4)
            {
                currencies = node["currencies"].ParseAXDataCollection<CurrencyAsset>(version).ToList();
            }
            else
            {
                currencies = new List<CurrencyAsset>();
            }

            if (version >= 5)
            {
                flags = node["flags"].ParseAXDataCollection<FlagDescriptionProjectAsset>(version).ToList();
            }
            else
            {
                flags = new List<FlagDescriptionProjectAsset>();
            }

            if (version >= 7 && version < 14)
            {
                var legacyDialogueVendors = node["dialogueVendors"].ParseAXDataCollection<VirtualDialogueVendor>(version).ToList();

                var convertedToDialogues = legacyDialogueVendors.Select(d => d.CreateDialogue());

                dialogues.AddRange(convertedToDialogues);
            }

            if (version >= 3)
            {
                lastCharacter = node["lastCharacter"].ToInt32();
                lastDialogue = node["lastDialogue"].ToInt32();
                lastVendor = node["lastVendor"].ToInt32();
                lastQuest = node["lastQuest"].ToInt32();

                if (version >= 4)
                {
                    lastCurrency = node["lastCurrency"].ToInt32();
                }
                else
                {
                    lastCurrency = -1;
                }

                if (version >= 7 && version < 14)
                {
                    var legacyLastDialogueVendor = node["lastDialogueVendor"].ToInt32();
                }
            }
            else
            {
                lastCharacter = -1;
                lastDialogue = -1;
                lastVendor = -1;
                lastQuest = -1;
                lastCurrency = -1;
            }

            if (version >= 6)
                settings.Load(node["settings"], version);
            else
                settings = new NPCProjectSettings();
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateAttributeC("guid", node).WriteString(guid);

            document.CreateNodeC("characters", node).WriteAXDataCollection(document, "NPCCharacter", characters);
            document.CreateNodeC("dialogues", node).WriteAXDataCollection(document, "NPCDialogue", dialogues);
            document.CreateNodeC("vendors", node).WriteAXDataCollection(document, "NPCVendor", vendors);
            document.CreateNodeC("quests", node).WriteAXDataCollection(document, "NPCQuest", quests);
            document.CreateNodeC("currencies", node).WriteAXDataCollection(document, "CurrencyAsset", currencies);
            document.CreateNodeC("flags", node).WriteAXDataCollection(document, "FlagDescriptionProjectAsset", flags);

            document.CreateNodeC("lastCharacter", node).WriteInt32(lastCharacter);
            document.CreateNodeC("lastDialogue", node).WriteInt32(lastDialogue);
            document.CreateNodeC("lastVendor", node).WriteInt32(lastVendor);
            document.CreateNodeC("lastQuest", node).WriteInt32(lastQuest);
            document.CreateNodeC("lastCurrency", node).WriteInt32(lastCurrency);

            settings.Save(document, document.CreateNodeC("settings", node));
        }
    }
    public class NPCProjectSettings : IAXData
    {
        public NPCProjectSettings()
        {
            assetDirs = new List<string>();
            idRangeMin = ushort.MinValue;
            idRangeMax = ushort.MaxValue;
        }

        public List<string> assetDirs;
        public ushort idRangeMin, idRangeMax;

        public void Load(XmlNode node, int version)
        {
            assetDirs = node["assetDirs"].ParseStringCollection().ToList();
            idRangeMin = node["idRangeMin"].ToUInt16(ushort.MinValue);
            idRangeMax = node["idRangeMax"].ToUInt16(ushort.MaxValue);
        }

        public void Save(XmlDocument document, XmlNode node)
        {
            document.CreateNodeC("assetDirs", node).WriteStringCollection(document, assetDirs);
            document.CreateNodeC("idRangeMin", node).WriteUInt16(idRangeMin);
            document.CreateNodeC("idRangeMax", node).WriteUInt16(idRangeMax);
        }
    }
}
