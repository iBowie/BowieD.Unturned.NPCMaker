using BowieD.Unturned.NPCMaker.Configuration;

namespace BowieD.Unturned.NPCMaker.Data
{
    public sealed class RecentFileList : JsonData<string[]>
    {
        public override string FileName => AppConfig.Directory + "recent.json";
    }
}
