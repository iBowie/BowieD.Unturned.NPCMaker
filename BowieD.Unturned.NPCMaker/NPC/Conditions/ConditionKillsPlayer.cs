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
        public ushort ID { get; set; }
        public short Value { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
    }
}
