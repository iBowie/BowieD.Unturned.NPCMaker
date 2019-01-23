namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    public class AssignmentOfficial : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.HIGH;
        public override bool IsMistake => MainWindow.Instance.Inputted_ID > 0 && MainWindow.Instance.Inputted_ID <= 2000;
        public override string MistakeNameKey => "mistakes_Mistake_General_AssignmentOfficial_Name";
        public override string MistakeDescKey => "mistakes_Mistake_General_AssignmentOfficial_Desc";
    }
}
