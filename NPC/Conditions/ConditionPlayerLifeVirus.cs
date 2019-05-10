using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionPlayerLifeVirus : Condition
    {
        public override Condition_Type Type => Condition_Type.Player_Life_Virus;
        public int Value;
        public Logic_Type Logic;
        public override string DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(MainWindow.Localize("Condition_Type_ConditionPlayerLifeVirus") + " ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        sb.Append("= ");
                        break;
                    case Logic_Type.Greater_Than:
                        sb.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        sb.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        sb.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        sb.Append("<= ");
                        break;
                    case Logic_Type.Not_Equal:
                        sb.Append("!= ");
                        break;
                }
                sb.Append(Value);
                return sb.ToString();
            }
        }
    }
}
