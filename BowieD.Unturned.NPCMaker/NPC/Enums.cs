using System.ComponentModel;

namespace BowieD.Unturned.NPCMaker.NPC
{
    public enum Condition_Type
    {
        None,
        Experience,
        Reputation,
        Quest,
        Item,
        Kills_Zombie,
        Kills_Horde,
        Kills_Animal,
        Kills_Player,
        Time_Of_Day,
        Player_Life_Health,
        Player_Life_Food,
        Player_Life_Water,
        Player_Life_Virus,
        Flag_Bool,
        Flag_Short,
        Skillset,
        Kills_Object,
        Holiday,
        Compare_Flags,
        Currency,
        Kills_Tree
    }
    public enum Logic_Type
    {
        Less_Than,
        Less_Than_Or_Equal_To,
        Equal,
        Not_Equal,
        Greater_Than_Or_Equal_To,
        Greater_Than
    }
    public enum ItemType
    {
        ITEM, VEHICLE
    }
    public enum NPC_Pose
    {
        [Description("Character.Pose_Stand")]
        Stand,
        [Description("Character.Pose_Sit")]
        Sit,
        [Description("Character.Pose_Asleep")]
        Asleep,
        [Description("Character.Pose_Passive")]
        Passive,
        [Description("Character.Pose_Crouch")]
        Crouch,
        [Description("Character.Pose_Prone")]
        Prone,
        [Description("Character.Pose_Under_Arrest")]
        Under_Arrest,
        [Description("Character.Pose_Rest")]
        Rest,
        [Description("Character.Pose_Surrender")]
        Surrender
    }
    public enum RewardType
    {
        None,
        Experience,
        Reputation,
        Quest,
        Item,
        Item_Random,
        Vehicle,
        Teleport,
        Flag_Bool,
        Flag_Short,
        Flag_Short_Random,
        Flag_Math,
        Achievement,
        Event,
        Currency,
        Hint
    }
    public enum Quest_Status
    {
        None,
        Active,
        Ready,
        Completed
    }
    public enum Equip_Type
    {
        [Description("Character.Equipment_Equipped_None")]
        None,
        [Description("Character.Equipment_Equipped_Primary")]
        Primary,
        [Description("Character.Equipment_Equipped_Secondary")]
        Secondary,
        [Description("Character.Equipment_Equipped_Tertiary")]
        Tertiary
    }
    public enum Zombie_Type
    {
        None,
        Normal,
        Mega,
        Crawler,
        Sprinter,
        Flanker_Friendly,
        Flanker_Stalk,
        Burner,
        Acid,
        Boss_Electric,
        Boss_Wind,
        Boss_Fire,
        Boss_All,
        Boss_Magma,
        Spirit,
        Boss_Spirit,
        Boss_Nuclear
    }
    public enum ESkillset
    {
        Fire,
        Police,
        Army,
        Farm,
        Fish,
        Camp,
        Work,
        Chef,
        Thief,
        Medic
    }
    public enum Modification_Type
    {
        Assign,
        Increment,
        Decrement
    }
    public enum Operation_Type
    {
        Assign,
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
    public enum ENPCHoliday
    {
        None,
        Halloween,
        Christmas
    }
    public enum ELanguage
    {
        English,
        Russian,
        Spanish,
        French
    }
    public enum ParseType
    {
        None,
        NPC,
        Dialogue,
        Vendor,
        Quest
    }
    public enum Clothing_Type
    {
        Default,
        Halloween,
        Christmas
    }
}
