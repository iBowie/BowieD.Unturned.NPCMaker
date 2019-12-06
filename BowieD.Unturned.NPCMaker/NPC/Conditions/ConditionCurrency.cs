using BowieD.Unturned.NPCMaker.Localization;
using System;

namespace BowieD.Unturned.NPCMaker.NPC.Conditions
{
    public sealed class ConditionCurrency : Condition
    {
        public override Condition_Type Type => Condition_Type.Currency;
        public override string UIText
        {
            get
            {
                string outp = LocalizationManager.Current.Condition["Type_Currency"] + " ";
                switch (Logic)
                {
                    case Logic_Type.Equal:
                        outp += "= ";
                        break;
                    case Logic_Type.Greater_Than:
                        outp += "> ";
                        break;
                    case Logic_Type.Greater_Than_Or_Equal_To:
                        outp += ">= ";
                        break;
                    case Logic_Type.Less_Than:
                        outp += "< ";
                        break;
                    case Logic_Type.Less_Than_Or_Equal_To:
                        outp += "<= ";
                        break;
                    case Logic_Type.Not_Equal:
                        outp += "!= ";
                        break;
                }
                outp += Value;
                return outp;
            }
        }
        public string GUID { get; set; }
        public Logic_Type Logic { get; set; }
        public uint Value { get; set; }
        [ConditionNoValue]
        public bool Reset { get; set; }
    }
}
