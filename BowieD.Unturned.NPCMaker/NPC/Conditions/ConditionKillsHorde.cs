using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsHorde : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Horde;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Horde"]} ({Nav}) >= {Value}";
            }
        }
        public ushort ID;
        public short Value;
        public byte Nav;
        [ConditionNoValue]
        public bool Reset;
    }
}
