using System;

namespace BowieD.Unturned.NPCMaker.NPC.Shared.Attributes
{
    public class TextBoxOptionsAttribute : Attribute
    {
        public TextBoxOptionsAttribute(params string[] options)
        {
            this.Options = options;
        }

        public string[] Options { get; }
    }
}
