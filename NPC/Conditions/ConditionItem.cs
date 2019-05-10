using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionItem : Condition
    {
        public override Condition_Type Type => Condition_Type.Item;
        public ushort ID;
        public ushort Amount;
        [ConditionNoValue]
        public bool Reset;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(MainWindow.Localize($"Condition_Type_ConditionItem") + " ");
                sb.Append($"{ID} x{Amount}");
                return sb.ToString();
            }
        }
    }
}
