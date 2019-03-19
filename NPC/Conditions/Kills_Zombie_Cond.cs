using System;
using System.Linq;
using BowieD.Unturned.NPCMaker.BetterForms;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public class Kills_Zombie_Cond : Condition
    {
        public Kills_Zombie_Cond()
        {
            Type = Condition_Type.Kills_Zombie;
        }

        public bool Spawn { get; set; }
        public ushort Id { get; set; }
        public ushort NavMesh { get; set; }
        public uint Amount { get; set; }
        public Zombie_Type Zombie_Type { get; set; }

        public override int Elements => 6;
        public override void Init(Universal_ConditionEditor uce)
        {
            uce.AddLabel(MainWindow.Localize("conditionEditor_Zombies"));
            uce.AddComboBox(Enum.GetValues(typeof(Zombie_Type)).Cast<Zombie_Type>(), "Zombie_{0}");
            uce.AddLabel(MainWindow.Localize("conditionEditor_ID"));
            uce.AddTextBox(5);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Amount"));
            uce.AddTextBox(6);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Spawn"));
            uce.AddCheckBox(false);
            uce.AddLabel(MainWindow.Localize("conditionEditor_Navmesh"));
            uce.AddTextBox(5);
            uce.AddResetLabelAndCheckbox("Kills_Zombie");
        }
        public override void Init(Universal_ConditionEditor uce, Condition start)
        {
            Init(uce);
            if (start != null)
            {
                uce.SetMainValue(1, (start as Kills_Zombie_Cond).Zombie_Type);
                uce.SetMainValue(3, (start as Kills_Zombie_Cond).Id);
                uce.SetMainValue(5, (start as Kills_Zombie_Cond).Amount);
                uce.SetMainValue(7, (start as Kills_Zombie_Cond).Spawn);
                uce.SetMainValue(9, (start as Kills_Zombie_Cond).NavMesh);
                uce.SetMainValue(11, start.Reset);
            }
        }
        public override T Parse<T>(object[] input)
        {
            return new Kills_Zombie_Cond
            {
                Zombie_Type = (Zombie_Type)input[0],
                Id = ushort.Parse(input[1].ToString()),
                Amount = uint.Parse(input[2].ToString()),
                Spawn = (bool)input[3],
                NavMesh = ushort.Parse(input[4].ToString()),
                Reset = (bool)input[5]
            } as T;
        }

        public override string GetFilePresentation(string prefix, int prefixIndex, int conditionIndex)
        {
            if (prefix.Length > 0)
                if (!prefix.EndsWith("_"))
                    prefix += "_";
            string output = "";
            output += ($"{prefix}{(prefix.Length > 0 ? $"{prefixIndex.ToString()}_" : "")}Condition_{conditionIndex}_Type Kills_Zombie");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Zombie {this.Zombie_Type}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Value {this.Amount}");
            output += ($"{Environment.NewLine}{prefix}{(prefix.Length > 0 ? $"{prefixIndex}_" : "")}Condition_{conditionIndex}_Nav {this.NavMesh}");
            return output;
        }
    }
}
