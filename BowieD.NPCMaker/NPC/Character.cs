using BowieD.NPCMaker.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.NPCMaker.NPC
{
    public sealed class Character
    {
        public string editorName;
        public string displayName;
        public ushort id;
        public ushort dialogueId;
        public byte face;
        public byte beard;
        public byte hair;
        public Coloring.Color hairColor;
        public Coloring.Color skinColor;
    }
    public sealed class Outfit
    {
        public ushort shirt, pants, hat, backpack, vest, mask, glasses;
    }
    public enum ESlotType
    {
        NONE,
        PRIMARY,
        SECONDARY,
        TERTIARY
    }
    public enum ENPCPose
    {
        STAND,
        SIT,
        ASLEEP,
        PASSIVE,
        CROUCH,
        PRONE,
        UNDER_ARREST,
        REST,
        SURRENDER
    }
}
