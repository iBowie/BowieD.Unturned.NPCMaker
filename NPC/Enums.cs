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
        Compare_Flags
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
        Stand,
        Sit,
        Asleep,
        Passive,
        Crouch,
        Prone,
        Under_Arrest,
        Rest,
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
        Event
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
        None,
        Primary,
        Secondary,
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
        Boss_Fire
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
    public enum Object_Type
    {
        SMALL,
        MEDIUM,
        LARGE
    }
    public enum Object_Interactability
    {
        None,
        Binary_State,
        Dropper,
        Note,
        Water,
        Fuel,
        Rubble,
        Quest
    }
    public enum Object_Rubble
    {
        None,
        Destroy
    }
    public enum Object_Hint
    {
        None,
        Door,
        Switch,
        Fire,
        Generator,
        Use
    }
}
