using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Animal_Cond : Condition
    {
        public Kills_Animal_Cond()
        {
            Type = Condition_Type.Kills_Animal;
        }

        public ushort Animal { get; set; }
        public ushort ID { get; set; }
        public uint Value { get; set; }

        public override int Elements => 4;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Animal"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Amount"));
            uce.AddTextBox(6);
            uce.AddResetLabelAndCheckbox("Kills_Animal");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Kills_Animal_Cond).ID);
                uce.SetMainValue(3, (start as Kills_Animal_Cond).Animal);
                uce.SetMainValue(5, (start as Kills_Animal_Cond).Value);
                uce.SetMainValue(7, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Kills_Animal_Cond
            {
                ID = ushort.Parse(input[0].ToString()),
                Animal = ushort.Parse(input[1].ToString()),
                Value = uint.Parse(input[2].ToString()),
                Reset = (bool)input[3]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Animal");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Animal {this.Animal}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.ID}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
