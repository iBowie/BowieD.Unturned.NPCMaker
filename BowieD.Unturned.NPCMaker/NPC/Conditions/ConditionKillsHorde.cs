using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionKillsHorde : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Horde;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Horde"]} ({Nav}) >= {Value}";
        public ushort ID { get; set; }
        public short Value { get; set; }
        public byte Nav { get; set; }

        public override bool Check(Simulation simulation)
        {
            if (simulation.Flags.TryGetValue(ID, out short flag))
            {
                return flag >= Value;
            }
            return false;
        }
        public override void Apply(Simulation simulation)
        {
            if (Reset)
            {
                simulation.Flags.Remove(ID);
            }
        }
        public override string FormatCondition(Simulation simulation)
        {
            string text = Localization;

            if (string.IsNullOrEmpty(text))
            {
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_HordeKills"];
            }

            if (!simulation.Flags.TryGetValue(ID, out short value))
            {
                value = 0;
            }

            return string.Format(text, value, Value);
        }

        public override void Load(System.Xml.XmlNode node, int version)
        {
            base.Load(node, version);

            ID = node["ID"].ToUInt16();
            Nav = node["Nav"].ToByte();
            Value = node["Value"].ToInt16();
        }

        public override void Save(System.Xml.XmlDocument document, System.Xml.XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Nav", node).WriteByte(Nav);
            document.CreateNodeC("Value", node).WriteInt16(Value);
        }
    }
}
