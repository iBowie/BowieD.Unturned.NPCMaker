using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using BowieD.Unturned.NPCMaker.NPC.Shared.Attributes;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    [Configuration.SkillLock(Configuration.ESkillLevel.Advanced)]
    public sealed class ConditionKillsZombie : Condition
    {
        public override Condition_Type Type => Condition_Type.Kills_Zombie;
        public override string UIText => $"[{ID}] {LocalizationManager.Current.Condition[$"Type_Kills_Zombie"]} : {LocalizationManager.Current.Condition[$"Kills_Zombie_Zombie_{Zombie}"]} ({Nav}) >= {Value} {(Spawn ? "Spawn" : "")}";
        public ushort ID { get; set; }
        public short Value { get; set; }
        [Optional(1)]
        [Range(0, int.MaxValue)]
        public int? Spawn_Quantity { get; set; }
        [NoValue]
        public bool Spawn { get; set; }
        [Optional(byte.MaxValue)]
        [Range((byte)0, (byte)254)]
        public byte? Nav { get; set; }
        [Range(0f, float.MaxValue)]
        public float Radius { get; set; } = 512f;
        [Range(0f, float.MaxValue)]
        public float MinRadius { get; set; }
        public Zombie_Type Zombie { get; set; }

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
                text = LocalizationManager.Current.Simulation["Quest"]["Default_Condition_ZombieKills"];
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
            Value = node["Value"].ToInt16();
            Spawn_Quantity = node["Spawn_Quantity"].ToNullableInt32();
            Spawn = node["Spawn"].ToBoolean();
            Nav = node["Nav"].ToNullableByte();
            Radius = node["Radius"].ToSingle();
            Zombie = node["Zombie"].ToEnum<Zombie_Type>();

            if (version >= 9)
            {
                MinRadius = node["MinRadius"].ToSingle();
            }
            else
            {
                MinRadius = 0f;
            }
        }

        public override void Save(System.Xml.XmlDocument document, System.Xml.XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("ID", node).WriteUInt16(ID);
            document.CreateNodeC("Value", node).WriteInt16(Value);
            document.CreateNodeC("Spawn_Quantity", node).WriteNullableInt32(Spawn_Quantity);
            document.CreateNodeC("Spawn", node).WriteBoolean(Spawn);
            document.CreateNodeC("Nav", node).WriteNullableByte(Nav);
            document.CreateNodeC("Radius", node).WriteSingle(Radius);
            document.CreateNodeC("Zombie", node).WriteEnum(Zombie);
            document.CreateNodeC("MinRadius", node).WriteSingle(MinRadius);
        }
    }
}
