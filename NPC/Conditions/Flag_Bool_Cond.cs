using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Flag_Bool_Cond : Condition
    {
        public Flag_Bool_Cond()
        {
            Type = Condition_Type.Flag_Bool;
        }
        
        public ushort Id { get; set; }
        public bool Value { get; set; }
        public bool AllowUnset { get; set; }
        public override int Elements => 4;

        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddCheckBox(false);
            uce.AddLabel(MainWindow.Localize("conditionEditor_AllowUnset"));
            uce.AddCheckBox(true);
            uce.AddResetLabelAndCheckbox("Flag_Bool");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Flag_Bool_Cond).Id);
                uce.SetMainValue(3, (start as Flag_Bool_Cond).Value);
                uce.SetMainValue(5, (start as Flag_Bool_Cond).AllowUnset);
                uce.SetMainValue(7, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Flag_Bool_Cond
            {
                Id = ushort.Parse(input[0].ToString()),
                Value = (bool)input[1],
                AllowUnset = (bool)input[2],
                Reset = (bool)input[3]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Flag_Bool");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic Equal");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            if (this.AllowUnset)
                output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Allow_Unset");
            return output;
        }
    }
}
