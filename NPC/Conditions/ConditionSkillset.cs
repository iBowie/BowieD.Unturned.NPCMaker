using System.Text;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    #endregion
    public sealed class ConditionSkillset : Condition
    {
        public override Condition_Type Type => Condition_Type.Skillset;
        public ESkillset Value;
        public Logic_Type Logic;
        public override string DisplayName
        {
            get
            {
                StringBuilder outp = new StringBuilder();
                outp.Append(MainWindow.Localize("Condition_Type_ConditionSkillset") + " ");
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp.Append("= ");
                        break;
                    case Logic_Type.Not_Equal:
                        outp.Append("!= ");
                        break;
                    case Logic_Type.Greater_Than:
                        outp.Append("> ");
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp.Append(">= ");
                        break;
                    case Logic_Type.Less_Than:
                        outp.Append("< ");
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp.Append("<= ");
                        break;
                }
                return outp.ToString();
            }
        }
    }
}
