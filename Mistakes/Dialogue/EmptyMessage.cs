using System.Linq;

namespace BowieD.Unturned.NPCMaker.Mistakes.Dialogue
{
    public class EmptyMessage : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.NO_EXPORT;
        public override bool IsMistake => MainWindow.dialogues?.Where(d => d.messages.Where(k => k.PagesAmount == 0).Count() > 0).Count() > 0;
        public override string MistakeNameKey => "mistakes_Mistake_Dialogue_EmptyMessage_Name";
        public override string MistakeDescKey => "mistakes_Mistake_Dialogue_EmptyMessage_Desc";
    }
}
