using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Skillset_Cond : Condition, Condition.IHasLogic, Condition.IHasValue<ESkillset>
    {
        public Skillset_Cond()
        {
            Type = Condition_Type.Skillset;
        }

        public Logic_Type Logic { get; set; }
        public NPC.ESkillset Value { get; set; }

        public override int Elements => 2;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_LogicType"));
            uce.AddLogicBox();
            uce.AddLabel(MainWindow.Localize("conditionEditor_Skillset"));
            uce.AddComboBox(Enum.GetValues(typeof(ESkillset)).Cast<ESkillset>(), "Skillset_{0}");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Skillset_Cond).Logic);
                uce.SetMainValue(3, (start as Skillset_Cond).Value);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Skillset_Cond
            {
                Logic = (Logic_Type)input[0],
                Value = (ESkillset)input[1]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Skillset");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
