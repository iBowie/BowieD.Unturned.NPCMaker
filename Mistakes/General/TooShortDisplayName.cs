namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    public class TooShortDisplayName : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake => MainWindow.Instance.Inputted_DisplayName.Length < 3;
        public override string MistakeNameKey => "mistakes_Mistake_General_TooShortDisplayName_Name";
        public override string MistakeDescKey => "mistakes_Mistake_General_TooShortDisplayName_Desc";
    }
}
