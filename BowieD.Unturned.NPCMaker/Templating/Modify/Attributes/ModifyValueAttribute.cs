using System;

namespace BowieD.Unturned.NPCMaker.Templating.Modify.Attributes
{
    public sealed class ModifyValueAttribute : Attribute
    {
        public ModifyValueAttribute(string id)
        {
            this.ID = id;
            this.InnerType = null;
        }
        public ModifyValueAttribute(string id, Type innerType)
        {
            this.ID = id;
            this.InnerType = innerType;
        }
        public string ID { get; }
        public Type InnerType { get; }
    }
}
