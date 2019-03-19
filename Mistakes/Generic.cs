namespace BowieD.Unturned.NPCMaker.Mistakes
{
    /// <summary>
    /// Generic mistake for Deep Analysis
    /// </summary>
    public class Generic : Mistake
    {
        private readonly string name;
        private readonly string desc;
        private readonly IMPORTANCE imp;
        private readonly bool isMis;
        private readonly bool transName;
        private readonly bool transDesc;

        public override string MistakeNameKey => name;
        public override string MistakeDescKey => desc;
        public override bool IsMistake => isMis;
        public override IMPORTANCE Importance => imp;
        public override bool TranslateName => transName;
        public override bool TranslateDesc => transDesc;

        public Generic(string nameKey, string descKey, IMPORTANCE imp, bool isMistake = false, bool translateName = true, bool translateDesc = true)
        {
            this.name = nameKey;
            this.desc = descKey;
            this.imp = imp;
            this.isMis = isMistake;
            this.transName = translateName;
            this.transDesc = translateDesc;
        }
    }
}
