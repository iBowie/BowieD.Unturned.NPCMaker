using BowieD.Unturned.NPCMaker.Data;

namespace BowieD.Unturned.NPCMaker.Managers
{
    public static class DataManager
    {
        private static RecentFileList recentFile;
        private static UserColorsList userColors;
        private static NPCNamePoolData namePool;

        public static RecentFileList RecentFileData 
        {
            get
            {
                if (recentFile == null)
                {
                    recentFile = new RecentFileList();
                    recentFile.Load(new string[0]);
                }
                return recentFile;
            }
        }
        public static UserColorsList UserColorsData
        {
            get
            {
                if (userColors == null)
                {
                    userColors = new UserColorsList();
                    userColors.Load(new string[0]);
                }
                return userColors;
            }
        }
    }
}
