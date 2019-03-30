using BowieD.Unturned.NPCMaker.NPC;
using System;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quests
{
    /// <summary>
    /// No conditions in quest
    /// </summary>
    public class NE_4002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.HIGH;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCQuest quest in MainWindow.CurrentNPC.quests)
                {
                    if (quest.conditions.Count == 0)
                    {
                        errorQuest = quest;
                        return true;
                    }
                }
                return false;
            }
        }
        private NPCQuest errorQuest;
        public override string MistakeDescKey => MainWindow.Localize("NE_4002_Desc", errorQuest.id);
        public override string MistakeNameKey => "NE_4002";
        public override bool TranslateDesc => false;
        public override bool TranslateName => false;
        public override Action OnClick => () =>
        {
            if (MainWindow.Instance.CurrentQuest.id == 0)
                return;
            MainWindow.Instance.SaveQuest_Click(null, null);
            MainWindow.Instance.CurrentQuest = errorQuest;
            MainWindow.Instance.mainTabControl.SelectedIndex = 4;
        };
    }
}
