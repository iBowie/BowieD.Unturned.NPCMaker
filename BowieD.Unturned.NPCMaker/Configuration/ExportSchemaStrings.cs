namespace BowieD.Unturned.NPCMaker.Configuration
{
    public static class ExportSchemaStrings
    {
        public const string
            SCHEMA_DEFAULT =
                "- {Project GUID}\n" +
                "\t- Characters\n" +
                "\t\t- {EditorName > GUID}_{ID}\n" +
                "\t- Dialogues\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Vendors\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Quests\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Currencies\n" +
                "\t\t- {GUID}.asset";

        public const string
            SCHEMA_VERBOSE_COMMENT =
                "- {Project GUID}\n" +
                "\t- Characters\n" +
                "\t\t- {Comment > EditorName > GUID}_{ID}\n" +
                "\t- Dialogues\n" +
                "\t\t- {Comment > GUID}_{ID}\n" +
                "\t- Vendors\n" +
                "\t\t- {Comment > Title > GUID}_{ID}\n" +
                "\t- Quests\n" +
                "\t\t- {Comment > Title > GUID}_{ID}\n" +
                "\t- Currencies\n" +
                "\t\t- {GUID}.asset";

        public const string
            SCHEMA_VERBOSE_NO_COMMENT =
                "- {Project GUID}\n" +
                "\t- Characters\n" +
                "\t\t- {EditorName > GUID}_{ID}\n" +
                "\t- Dialogues\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Vendors\n" +
                "\t\t- {Title > GUID}_{ID}\n" +
                "\t- Quests\n" +
                "\t\t- {Title > GUID}_{ID}\n" +
                "\t- Currencies\n" +
                "\t\t- {GUID}.asset";

        public const string
            SCHEMA_GUID_ALL_THE_WAY =
                "- {Project GUID}\n" +
                "\t- Characters\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Dialogues\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Vendors\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Quests\n" +
                "\t\t- {GUID}_{ID}\n" +
                "\t- Currencies\n" +
                "\t\t- {GUID}.asset";
    }
}
