using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Quest_Cond : Condition
    {
        public Quest_Cond()
        {
            Type = Condition_Type.Quest;
        }

        public ushort Id { get; set; }
        public Quest_Status Status { get; set; }
        public Logic_Type Logic { get; set; }

        public override int Elements => 4;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_LogicType"));
            uce.AddLogicBox();
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Status"));
            uce.AddComboBox(Enum.GetValues(typeof(Quest_Status)).Cast<Quest_Status>(), "QuestStatus_{0}");
            uce.AddResetLabelAndCheckbox("Quest");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Quest_Cond).Logic);
                uce.SetMainValue(3, (start as Quest_Cond).Id);
                uce.SetMainValue(5, (start as Quest_Cond).Status);
                uce.SetMainValue(7,  start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Quest_Cond
            {
                Logic = (Logic_Type)input[0],
                Id = ushort.Parse(input[1].ToString()),
                Status = (Quest_Status)input[2],
                Reset = (bool)input[3]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Quest");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_ID {this.Id}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Status {this.Status}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Logic {this.Logic}");
            return output;
        }
    }
}
