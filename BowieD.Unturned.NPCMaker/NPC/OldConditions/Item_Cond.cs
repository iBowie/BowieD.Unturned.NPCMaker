using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Item_Cond : Condition
    {
        public Item_Cond()
        {
            Type = Condition_Type.Item;
        }

        public ushort Id { get; set; }
        public uint Amount { get; set; }

        public override int Elements => 3;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Amount"));
            uce.AddTextBox(6);
            uce.AddResetLabelAndCheckbox("Item");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Item_Cond).Id);
                uce.SetMainValue(3, (start as Item_Cond).Amount);
                uce.SetMainValue(5, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Item_Cond
            {
                Id = ushort.Parse(input[0].ToString()),
                Amount = uint.Parse(input[1].ToString()),
                Reset = (bool)input[2]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Item");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Amount {this.Amount}");
            return output;
        }
    }
}
