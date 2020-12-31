using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionKillsAnimal : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Animal;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Animal"]} ({Animal}) >= {Value}";
        public ushort ID { get; set; }
        [AssetPicker(typeof(GameAnimalAsset), "Control_SelectAsset_Animal", MahApps.Metro.IconPacks.PackIconMaterialKind.Bone)]
        public ushort Animal { get; set; }
        public short Value { get; set; }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out short flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_AnimalKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            return string.Format(text, value, Value);
        }
    }
}
