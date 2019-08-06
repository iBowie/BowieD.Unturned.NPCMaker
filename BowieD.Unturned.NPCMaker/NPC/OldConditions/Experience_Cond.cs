using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Experience_Cond : Condition, Condition.IHasLogic, Condition.IHasValue<uint>
    {
        public Experience_Cond()
        {
            Type = Condition_Type.Experience;
        }

        public uint Value { get; set; }
        public Logic_Type Logic { get; set; }

        public override int Elements => 3;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_LogicType"));
            uce.AddLogicBox();
            uce.AddLabel(MainWindow.Localize("conditionEditor_Amount"));
            uce.AddTextBox(6);
            uce.AddResetLabelAndCheckbox("Experience");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Experience_Cond).Logic);
                uce.SetMainValue(3, (start as Experience_Cond).Value);
                uce.SetMainValue(5, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Experience_Cond()
            {
                Logic = (Logic_Type)input[0],
                Value = uint.Parse(input[1].ToString()),
                Reset = (bool)input[2]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Experience");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
