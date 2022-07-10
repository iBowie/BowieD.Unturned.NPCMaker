using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Mistakes
{
    public static class ConditionChecker
    {
        private static readonly Dictionary<Type, List<NPC.Condition_Type>> _disallowedTypes = new Dictionary<Type, List<NPC.Condition_Type>>();

        static ConditionChecker()
        {
            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Animal);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Animal);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Animal);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Animal);

            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Horde);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Horde);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Horde);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Horde);

            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Object);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Object);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Object);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Object);

            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Player);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Player);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Player);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Player);

            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Tree);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Tree);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Tree);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Tree);

            Disallow<NPC.NPCCharacter>(NPC.Condition_Type.Kills_Zombie);
            Disallow<NPC.NPCMessage>(NPC.Condition_Type.Kills_Zombie);
            Disallow<NPC.NPCResponse>(NPC.Condition_Type.Kills_Zombie);
            Disallow<NPC.VendorItem>(NPC.Condition_Type.Kills_Zombie);
        }

        private static void Disallow<TParent>(NPC.Condition_Type conditionType)
        {
            var parent = typeof(TParent);

            if (!_disallowedTypes.ContainsKey(parent))
            {
                _disallowedTypes[parent] = new List<NPC.Condition_Type>();
            }

            var lst = _disallowedTypes[parent];

            lst.Add(conditionType);
        }

        public static bool IsAllowed<TParent>(NPC.Condition_Type conditionType)
        {
            return IsAllowed(typeof(TParent), conditionType);
        }

        public static bool IsAllowed(Type parent, NPC.Condition_Type conditionType)
        {
            if (!_disallowedTypes.TryGetValue(parent, out var lst))
                return true;

            if (!lst.Contains(conditionType))
                return true;

            return false;
        }
    }
}
