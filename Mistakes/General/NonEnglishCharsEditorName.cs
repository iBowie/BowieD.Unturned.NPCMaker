using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.General
{
    public class NonEnglishCharsEditorName : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake => MainWindow.Instance.Inputted_EditorName.Count(d => !d.IsEnglishChar()) > 0;
        public override string MistakeDescKey => "mistakes_Mistake_General_NonEnglishCharsEditorName_Desc";
        public override string MistakeNameKey => "mistakes_Mistake_General_NonEnglishCharsEditorName_Name";
    }
}
