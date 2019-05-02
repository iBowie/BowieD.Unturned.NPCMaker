using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Object_Cond : Condition, Condition.IHasValue<short>
    {
        public Kills_Object_Cond()
        {
            Type = Condition_Type.Kills_Object;
        }

        public ushort ID { get; set; }
        public short Value { get; set; }
        public Guid Object { get; set; }
        public byte Nav { get; set; }

        public override int Elements => 5;

        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(6);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddTextBox(6);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Object"));
            uce.AddTextBox(32);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Navmesh") + "*", MainWindow.Localize("conditionEditor_Navmesh_Objects"));
            uce.AddTextBox(3);
            uce.AddResetLabelAndCheckbox("Kills_Objects");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Kills_Object_Cond).ID);
                uce.SetMainValue(3, (start as Kills_Object_Cond).Value);
                uce.SetMainValue(5, (start as Kills_Object_Cond).Object);
                uce.SetMainValue(7, (start as Kills_Object_Cond).Nav);
                uce.SetMainValue(9, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Kills_Object_Cond()
            {
                ID = ushort.Parse(input[0].ToString()),
                Value = short.Parse(input[1].ToString()),
                Object = Guid.Parse(input[2].ToString()),
                Nav = byte.Parse(input[3].ToString())
            } as T;
        }
        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Object");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.ID}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Object {this.Object.ToString("N")}");
            if (this.Nav != byte.MaxValue)
                output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Nav {this.Nav}");
            return output;
        }
    }
}
