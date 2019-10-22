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
        public ushort ID { get; set; }
        public short Value { get; set; }
        public byte Nav { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
    }
}
