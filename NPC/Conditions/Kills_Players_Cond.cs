using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Players_Cond : Condition
    {
        public Kills_Players_Cond()
        {
            Type = Condition_Type.Kills_Player;
        }

        public ushort ID { get; set; }
        public short Value { get; set; }

        public override int Elements => 3;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddTextBox(5);
            uce.AddResetLabelAndCheckbox("Kills_Player");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Kills_Players_Cond).ID);
                uce.SetMainValue(3, (start as Kills_Players_Cond).Value);
                uce.SetMainValue(5,  start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Kills_Players_Cond()
            {
                ID = ushort.Parse(input[0].ToString()),
                Value = short.Parse(input[1].ToString()),
                Reset = (bool)input[2]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Players");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.ID}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
