using BowieD.Unturned.NPCMaker.Localization;
using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionItem : Condition
    {
        public override Condition_Type Type => Condition_Type.Item;
        public ushort ID { get; set; }
        public ushort Amount { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(LocalizationManager.Current.Condition[$"Type_Item"] + " ");
                sb.Append($"{ID} x{Amount}");
                return sb.ToString();
            }
        }
    }
}
