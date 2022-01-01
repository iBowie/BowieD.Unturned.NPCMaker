using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public struct FindReplaceFormat
    {
        public FindReplaceFormat(string formatID, Type valueType)
        {
            FormatID = formatID;
            ValueType = valueType;
        }

        public string FormatID { get; }
        public Type ValueType { get; }

        public string DisplayFormatName
        {
            get
            {
                return Localization.LocalizationManager.Current.Interface.Translate($"FindReplace_Format_{FormatID}");
            }
        }
    }
    public static class FindReplaceFormats
    {
        public static readonly FindReplaceFormat CHARACTER_ID = new FindReplaceFormat("Character_ID", typeof(ushort));
        public static readonly FindReplaceFormat CHARACTER_GUID = new FindReplaceFormat("Character_GUID", typeof(string));
        public static readonly FindReplaceFormat DIALOGUE_ID = new FindReplaceFormat("Dialogue_ID", typeof(ushort));
        public static readonly FindReplaceFormat DIALOGUE_GUID = new FindReplaceFormat("Dialogue_GUID", typeof(string));
        public static readonly FindReplaceFormat ITEM_ID = new FindReplaceFormat("Item_ID", typeof(ushort));
        public static readonly FindReplaceFormat ITEM_OPTIONAL_ID = new FindReplaceFormat("Item_Optional_ID", typeof(ushort?));
        public static readonly FindReplaceFormat ITEM_GUIDID = new FindReplaceFormat("Item_GUIDID", typeof(GUIDIDBridge));
        public static readonly FindReplaceFormat VENDOR_ID = new FindReplaceFormat("Vendor_ID", typeof(ushort));
        public static readonly FindReplaceFormat VENDOR_GUID = new FindReplaceFormat("Vendor_GUID", typeof(string));
        public static readonly FindReplaceFormat QUEST_ID = new FindReplaceFormat("Quest_ID", typeof(ushort));
        public static readonly FindReplaceFormat QUEST_GUID = new FindReplaceFormat("Quest_GUID", typeof(string));
        public static readonly FindReplaceFormat VEHICLE_ID = new FindReplaceFormat("Vehicle_ID", typeof(ushort));
        public static readonly FindReplaceFormat VEHICLE_SPAWNPOINT = new FindReplaceFormat("Vehicle_Spawnpoint", typeof(string));
        public static readonly FindReplaceFormat FLAG_ID = new FindReplaceFormat("Flag_ID", typeof(ushort));
        public static readonly FindReplaceFormat CURRENCY_GUID = new FindReplaceFormat("Currency_GUID", typeof(string));
    }
}
