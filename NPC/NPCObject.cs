using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public class NPCObject
    {
        public string guid; // GUID
        public Object_Type type; // Type
        public ushort ID { get; set; } // ID
        public bool isFuelSource; // Fuel
        public bool isWaterSource; // Refill
        public bool noVehicleDamage; // Soft
        public Object_Interactability interactability; // Interactability
        public List<Condition> conditions; // Condition_#
        public string hint; // Interactability_Hint
        public List<string> noteLines; // Interactability_Text_Line_#
        public bool requireElectricity; // Interactability_Power
        public List<Reward> rewards; // Interactability_Reward_#
        public List<ushort> drops; // Interactability_Drop_#
        public bool interactInEditor; // Interactability_Editor
        public float timeBeforeReset; // Interactability_Reset
        public Object_Rubble rubble; // Rubble
        public ushort rubbleHealth; // Rubble_Health
        public ushort rubbleEffect; // Rubble_Effect
        public ushort rubbleFinaleEffect; // Rubble_Finale
        public float rubbleReset; // Rubble_Reset
        public bool hasLOD; // LOD
        public float LOD_Biad; // LOD_Bias

        public NPCObject()
        {
            guid = Guid.NewGuid().ToString("N");
            type = Object_Type.MEDIUM;
            ID = 0;
            isFuelSource = false;
            isWaterSource = false;
            noVehicleDamage = false;
            interactability = Object_Interactability.None;
            conditions = new List<Condition>();
            hint = "";
            noteLines = new List<string>();
            requireElectricity = false;
            rewards = new List<Reward>();
            drops = new List<ushort>();
            interactInEditor = false;
            timeBeforeReset = 0;
            rubble = Object_Rubble.None;
            rubbleHealth = 0;
            rubbleEffect = 0;
            rubbleFinaleEffect = 0;
            rubbleReset = 0;
        }
    }
}
