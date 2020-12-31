using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionKillsObject : Condition
    {
        public ushort ID { get; set; }
        public short Value { get; set; }
        [AssetPicker(typeof(GameObjectAsset), "Control_SelectAsset_Object", MahApps.Metro.IconPacks.PackIconMaterialKind.Home)]
        public string Object { get; set; }
        [Optional(byte.MaxValue)]
        public byte? Nav { get; set; }
        public override Condition_Type Type => Condition_Type.Kills_Object;
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{ID}] {Object} x{Value} {{{Nav}}}");
                return sb.ToString();
            }
        }

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
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_ObjectKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            return string.Format(text, value, Value);
        }
    }
}
