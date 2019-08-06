using BowieD.Unturned.NPCMaker.NPC;
using System;
using BowieD.Unturned.NPCMaker.Localization;

namespace BowieD.Unturned.NPCMaker.Mistakes.Quests
{
    /// <summary>
    /// No conditions in quest
    /// </summary>
    public class NE_4002 : Mistake
    {
        public override IMPORTANCE Importance => IMPORTANCE.WARNING;
        public override bool IsMistake
        {
            get
            {
                foreach (NPCQuest quest in MainWindow.CurrentProject.quests)
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
        public override string MistakeDescKey => LocUtil.LocalizeMistake("NE_4002_Desc", errorQuest.id);
        public override string MistakeNameKey => "NE_4002";
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
