using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Animal"]} ({Animal}) >= {Value}";
        public ushort ID { get; set; }
        public ushort Animal { get; set; }
        public short Value { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
    }
}
