using System;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Player_Life_Water_Cond : Condition, Condition.IHasLogic, Condition.IHasValue<ushort>
    {
        public Player_Life_Water_Cond()
        {
            Type = Condition_Type.Player_Life_Water;
        }

        public Logic_Type Logic { get; set; }
        public ushort Value { get; set; }

        public override int Elements => 2;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_LogicType"));
            uce.AddLogicBox();
            uce.AddLabel(MainWindow.Localize("conditionEditor_Value"));
            uce.AddTextBox(6);
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Player_Life_Water_Cond).Logic);
                uce.SetMainValue(3, (start as Player_Life_Water_Cond).Value);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Player_Life_Water_Cond
            {
                Logic = (Logic_Type)input[0],
                Value = ushort.Parse(input[1].ToString())
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Player_Life_Water");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Value}");
            return output;
        }
    }
}
