using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.GameIntegration;
using BowieD.Unturned.NPCMaker.Localization;
using System.Linq;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionItem : Condition
    {
        public override Condition_Type Type => Condition_Type.Item;
        [ConditionAssetPicker(typeof(GameItemAsset))]
        public ushort ID { get; set; }
        public ushort Amount { get; set; }
        public override string UIText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(LocalizationManager.Current.Condition[$"Type_Item"] + " ");
                if (GameAssetManager.TryGetAsset<GameItemAsset>(ID, out var asset))
                {
                    sb.Append($"{asset.name} x{Amount}");
                }
                else
                {
                    sb.Append($"{ID} x{Amount}");
                }
                return sb.ToString();
            }
        }

        public override bool Check(Simulation simulation)
        {
            System.Collections.Generic.List<Simulation.Item> items = simulation.Items.Where(d => d.ID == ID).ToList();

            return SimulationTool.Compare(items.Count, Amount, Logic_Type.Greater_Than_Or_Equal_To);
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                foreach (Simulation.Item i in simulation.Items.Where(d => d.ID == ID).Take(Amount).ToList())
                {
                    simulation.Items.Remove(i);
                }
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"].Translate("Default_Condition_Item");
            }

            System.Collections.Generic.IEnumerable<Simulation.Item> found = simulation.Items.Where(d => d.ID == ID);

            return string.Format(text, found.Count(), Amount, ID);
        }
    }
}
