using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string GameName
        {
            get
            {
                return $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Animal"]} ({Animal}) >= {Value}";
            }
        }
        public ushort ID;
        public ushort Animal;
        public short Value;
        [ConditionNoValue]
        public bool Reset;
    }
}
