using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Flag_Short_Cond : Condition, Condition.IHasLogic, Condition.IHasValue<short>
    {
        public Flag_Short_Cond()
        {
            Type = Condition_Type.Flag_Short;
        }

        public Logic_Type Logic { get; set; }
        public ushort Id { get; set; }
        public short Value { get; set; }
        public bool AllowUnset { get; set; }

        public override int Elements => 5;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_LogicType"));
            uce.AddLogicBox();
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_AllowUnset"));
            uce.AddCheckBox(true);
            uce.AddResetLabelAndCheckbox("Flag_Short");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);   
            if (start != null)
            {
                uce.SetMainValue(1, (start as Flag_Short_Cond).Id);
                uce.SetMainValue(3, (start as Flag_Short_Cond).Logic);
                uce.SetMainValue(5, (start as Flag_Short_Cond).Value);
                uce.SetMainValue(7, (start as Flag_Short_Cond).AllowUnset);
                uce.SetMainValue(9, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Short_Cond
            {
                Id = ushort.Parse(input[0].ToString()),
                Logic = (Logic_Type)input[1],
                Value = short.Parse(input[2].ToString()),
                AllowUnset = (bool)input[3],
                Reset = (bool)input[4]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Flag_Short");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            if (this.AllowUnset)
                output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Allow_Unset");
            return output;
        }
    }
}
