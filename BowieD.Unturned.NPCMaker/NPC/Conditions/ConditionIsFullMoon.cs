using BowieD.Unturned.NPCMaker.Common;
using BowieD.Unturned.NPCMaker.Localization;
using System.Xml;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    [System.Serializable]
    public sealed class ConditionIsFullMoon : Condition
    {
        private bool _isFullMoon;

        public ConditionIsFullMoon()
        {
            _isFullMoon = true;
        }

        public bool Value
        {
            get => _isFullMoon;
            set => _isFullMoon = value;
        }

        public override Condition_Type Type => Condition_Type.Is_Full_Moon;

        public override string UIText
        {
            get
            {
                string key;

                if (Value)
                    key = "Is_Full_Moon_DisplayWhenChecked";
                else
                    key = "Is_Full_Moon_DisplayWhenNotChecked";

                return LocalizationManager.Current.Condition[key];
            }
        }

        public override void Load(XmlNode node, int version)
        {
            base.Load(node, version);

            Value = node["Value"].ToBoolean();
        }

        public override void Save(XmlDocument document, XmlNode node)
        {
            base.Save(document, node);

            document.CreateNodeC("Value", node).WriteBoolean(Value);
        }
    }
}
