using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quests
{
    public class NE_4003 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.ADVICE;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCQuest quest in MainWindow.CurrentNPC.quests)
                {
                    if (quest.rewards.Count == 0)
                    {
                        errorQuest = quest;
                        return true;
                    }
                }
                return false;
            }
        }
        private NPCQuest errorQuest;
        public override string MistakeDescKey => MainWindow.Localize("NE_4003_Desc", errorQuest.id);
        public override string MistakeNameKey => "NE_4003";
        public override bool TranslateDesc => false;
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            if (MainWindow.QuestEditor.Current.id == 0)
                return;
            MainWindow.QuestEditor.Save();
            MainWindow.QuestEditor.Current = errorQuest;
            MainWindow.Instance.mainTabControl.SelectedIndex = 4;
        };
    }
}
