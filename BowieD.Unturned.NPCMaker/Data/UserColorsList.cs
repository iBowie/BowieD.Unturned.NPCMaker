using BowieD.Unturned.NPCMaker.Configuration;

namespace BowieD.Unturned.NPCMaker.Data
{
    public sealed class UserColorsList : JsonData<string[]>
    {
        public override string FileName => AppConfig.Directory + "colors.json";
    }
}
