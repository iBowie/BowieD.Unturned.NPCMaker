namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public class Generic : Mistake
    {
        private readonly string name;
        private readonly string desc;
        private readonly IMPORTANCE imp;
        private readonly bool isMis;

        public override string MistakeNameKey => name;
        public override string MistakeDescKey => desc;
        public override bool IsMistake => isMis;
        public override IMPORTANCE Importance => imp;

        public Generic(string nameKey, string descKey, IMPORTANCE imp, bool isMistake = false)
        {
            this.name = nameKey;
            this.desc = descKey;
            this.imp = imp;
            this.isMis = isMistake;
        }
    }
}
