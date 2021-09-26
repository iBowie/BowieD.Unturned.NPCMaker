using System.ComponentModel;

namespace BowieD.Unturned.NPCMaker.Configuration
{
    public enum EExportSchema
    {
        /// <summary>
        /// Default <para/>
        /// Project GUID
        /// <para/> - Characters
        /// <para/> - - {EditorName}_{ID}
        /// <para/> - Dialogues
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Vendors
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Quests
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Currencies
        /// <para/> - - {GUID}.asset
        /// </summary>
        [Description("Options.exportSchema_Option_Default")]
        Default = 0,
        /// <summary>
        /// The most unsafe (only for those who know what they are doing) <para/>
        /// Project GUID
        /// <para/> - Characters
        /// <para/> - - {EditorName}_{ID}
        /// <para/> - Dialogues
        /// <para/> - - {Comment ?? GUID}_{ID}
        /// <para/> - Vendors
        /// <para/> - - {Comment ?? Title ?? GUID}_{ID}
        /// <para/> - Quests
        /// <para/> - - {Comment ?? Title ?? GUID}_{ID}
        /// <para/> - Currencies
        /// <para/> - - {GUID}.asset
        /// </summary>
        [Description("Options.exportSchema_Option_Verbose_Comment")]
        Verbose_Comment = 1,
        /// <summary>
        /// The most unsafe (only for those who know what they are doing) <para/>
        /// Project GUID
        /// <para/> - Characters
        /// <para/> - - {EditorName}_{ID}
        /// <para/> - Dialogues
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Vendors
        /// <para/> - - {Title ?? GUID}_{ID}
        /// <para/> - Quests
        /// <para/> - - {Title ?? GUID}_{ID}
        /// <para/> - Currencies
        /// <para/> - - {GUID}.asset
        /// </summary>
        [Description("Options.exportSchema_Option_Verbose_No_Comment")]
        Verbose_No_Comment = 2,
        /// <summary>
        /// Safest one <para/>
        /// Project GUID
        /// <para/> - Characters
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Dialogues
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Vendors
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Quests
        /// <para/> - - {GUID}_{ID}
        /// <para/> - Currencies
        /// <para/> - - {GUID}.asset
        /// </summary>
        [Description("Options.exportSchema_Option_GUID_All_The_Way")]
        GUID_All_The_Way = 3
    }
}
