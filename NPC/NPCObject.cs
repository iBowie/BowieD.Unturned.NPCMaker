//using System;
//using System.Collections.Generic;

//namespace BowieD.Unturned.NPCMaker.NPC
//{
//    public class NPCObject
//    {
//        public string guid; // GUID
//        public Object_Type type; // Type
//        public string name;
//        public ushort ID { get; set; } // ID
//        public bool isFuelSource; // Fuel
//        public bool isWaterSource; // Refill
//        public bool noVehicleDamage; // Soft
//        public Object_Interactability interactability; // Interactability
//        public List<Condition> conditions; // Condition_#
//        public Object_Hint hint; // Interactability_Hint
//        public List<string> noteLines; // Interactability_Text_Line_#
//        public bool requireElectricity; // Interactability_Power
//        /// <summary>
//        /// Interactability_Reward_#
//        /// </summary>
//        public List<Reward> rewards;
//        /// <summary>
//        /// Interactability_Reward_ID
//        /// </summary>
//        public ushort interactDrop;
//        ///// <summary>
//        /// Interactability_Drop_#
//        /// </summary>
//        //public List<ushort> drops;
//        /// <summary>
//        /// Interactability_Editor
//        /// </summary>
//        public bool interactInEditor;
//        /// <summary>
//        /// Interactability_Health
//        /// </summary>
//        public ushort interactHealth;
//        /// <summary>
//        /// Interactability_Effect
//        /// </summary>
//        public ushort interactEffect;
//        /// <summary>
//        /// Interactability_Delay
//        /// </summary>
//        public float interactDelay;
//        /// <summary>
//        /// Interactability_Reset
//        /// </summary>
//        public float interactReset;
//        /// <summary>
//        /// Interactability_Resource
//        /// </summary>
//        public ushort interactResource;
//        public bool IsRubble;
//        public ushort RubbleReset;
//        public ushort RubbleHealth;
//        public ushort RubbleEffect;
//        public string assetFilePath;

//        public NPCObject()
//        {
//            guid = Guid.NewGuid().ToString("N");
//            name = "";
//            type = Object_Type.MEDIUM;
//            ID = 0;
//            isFuelSource = false;
//            isWaterSource = false;
//            noVehicleDamage = false;
//            interactability = Object_Interactability.None;
//            conditions = new List<Condition>();
//            hint = Object_Hint.None;
//            noteLines = new List<string>();
//            requireElectricity = false;
//            rewards = new List<Reward>();
//            //drops = new List<ushort>();
//            interactDrop = 0;
//            interactInEditor = false;
//            interactHealth = 0;
//            interactEffect = 0;
//            interactDelay = 0;
//            interactReset = 0;
//            assetFilePath = "";
//            RubbleEffect = 0;
//            RubbleHealth = 0;
//            RubbleReset = 0;
//            IsRubble = false;
//            interactResource = 0;
//        }

//        public override string ToString()
//        {
//            return $"{ID}";
//        }
//    }
//}
