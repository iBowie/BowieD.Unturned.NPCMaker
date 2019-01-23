namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class QuestSetting : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.HIGH;
        public override string MistakeNameKey => "mistakes_Mistake_Dialogue_QuestSetting_Name";
        public override string MistakeDescKey => "mistakes_Mistake_Dialogue_QuestSetting_Desc";
        public override bool IsMistake
        {
            get
            {
                return false;
            }
        }
    }
}
