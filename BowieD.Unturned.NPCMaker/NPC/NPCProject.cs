using BowieD.Unturned.NPCMaker.Common;
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
        public const int CURRENT_SAVEDATA_VERSION = 7;

        public NPCProject()
        {
            guid = Guid.NewGuid().ToString("N");
            SAVEDATA_VERSION = CURRENT_SAVEDATA_VERSION;
            characters = new List<NPCCharacter>();
            dialogues = new List<NPCDialogue>();
            vendors = new List<NPCVendor>();
            dialogueVendors = new List<VirtualDialogueVendor>();
            quests = new List<NPCQuest>();
            currencies = new List<CurrencyAsset>();
            flags = new List<FlagDescriptionProjectAsset>();
            lastCharacter = -1;
            lastDialogue = -1;
            lastVendor = -1;
            lastDialogueVendor = -1;
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
        public List<VirtualDialogueVendor> dialogueVendors;
        public List<NPCQuest> quests;
        public List<CurrencyAsset> currencies;
        public List<FlagDescriptionProjectAsset> flags;
        public int
            lastCharacter = -1,
            lastDialogue = -1,
            lastVendor = -1,
            lastDialogueVendor = -1,
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
                        Backpack = node["backpack"].ToUInt16(),
                        Hat = node["hat"].ToUInt16(),
                        Mask = node["mask"].ToUInt16(),
                        Shirt = node["top"].ToUInt16(),
                        Pants = node["bottom"].ToUInt16(),
                        Vest = node["vest"].ToUInt16()
                    },
                    startDialogueId = node["startDialogueId"].ToUInt16(),
                    pose = node["pose"].ToEnum<NPC_Pose>(),
                    equipped = node["equipped"].ToEnum<Equip_Type>(),
                    equipPrimary = node["equipPrimary"].ToUInt16(),
                    equipSecondary = node["equipSecondary"].ToUInt16(),
                    equipTertiary = node["equipTertiary"].ToUInt16(),
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

            if (version >= 7)
            {
                dialogueVendors = node["dialogueVendors"].ParseAXDataCollection<VirtualDialogueVendor>(version).ToList();
            }
            else
            {
                dialogueVendors = new List<VirtualDialogueVendor>();
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

                if (version >= 7)
                {
                    lastDialogueVendor = node["lastDialogueVendor"].ToInt32();
                }
                else
                {
                    lastDialogueVendor = -1;
                }
            }
            else
            {
                lastCharacter = -1;
                lastDialogue = -1;
                lastVendor = -1;
                lastDialogueVendor = -1;
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
            document.CreateNodeC("dialogueVendors", node).WriteAXDataCollection(document, "VirtualDialogueVendor", dialogueVendors);
            document.CreateNodeC("quests", node).WriteAXDataCollection(document, "NPCQuest", quests);
            document.CreateNodeC("currencies", node).WriteAXDataCollection(document, "CurrencyAsset", currencies);
            document.CreateNodeC("flags", node).WriteAXDataCollection(document, "FlagDescriptionProjectAsset", flags);

            document.CreateNodeC("lastCharacter", node).WriteInt32(lastCharacter);
            document.CreateNodeC("lastDialogue", node).WriteInt32(lastDialogue);
            document.CreateNodeC("lastVendor", node).WriteInt32(lastVendor);
            document.CreateNodeC("lastDialogueVendor", node).WriteInt32(lastDialogueVendor);
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
