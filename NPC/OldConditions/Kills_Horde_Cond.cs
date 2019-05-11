using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Horde_Cond : Condition, Condition.IHasValue<short>
    {
        public Kills_Horde_Cond()
        {
            Type = Condition_Type.Kills_Horde;
        }
        
        public ushort FlagID { get; set; }
        public short Value { get; set; }
        public ushort Navmesh { get; set; }

        public override int Elements => 4;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddTextBox(6);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Navmesh"));
            uce.AddTextBox(5);
            uce.AddResetLabelAndCheckbox("Kills_Horde");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Kills_Horde_Cond).FlagID);
                uce.SetMainValue(3, (start as Kills_Horde_Cond).Value);
                uce.SetMainValue(5, (start as Kills_Horde_Cond).Navmesh);
                uce.SetMainValue(7, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Kills_Horde_Cond
            {
                FlagID = ushort.Parse(input[0].ToString()),
                Value = short.Parse(input[1].ToString()),
                Navmesh = ushort.Parse(input[2].ToString()),
                Reset = (bool)input[3]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Horde");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Nav {this.Navmesh}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.FlagID}");
            return output;
        }
    }
}
