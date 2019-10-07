using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsPlayer : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Player;
        public override string DisplayName
        {
            get
            {
                return $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Player"]} >= {Value}";
            }
        }
        public ushort ID;
        public short Value;
        [ConditionNoValue]
        public bool Reset;
    }
}
