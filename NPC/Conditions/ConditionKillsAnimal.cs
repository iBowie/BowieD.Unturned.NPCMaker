using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {LocUtil.LocalizeCondition("Condition_Type_ConditionKillsAnimal")} ({Animal}) >= {Value}";
            }
        }
        public ushort ID; 
        public ushort Animal;
        public short Value;
        [ConditionNoValue]
        public bool Reset;
    }
}
