using BowieD.Unturned.NPCMaker.Configuration;
using System.IO;

namespace BowieD.Unturned.NPCMaker.Data
{
    public sealed class UserColorsList : JsonData<string[]>
    {
        public override string FileName => Path.Combine(AppConfig.Directory, "colors.json");
    }
}
