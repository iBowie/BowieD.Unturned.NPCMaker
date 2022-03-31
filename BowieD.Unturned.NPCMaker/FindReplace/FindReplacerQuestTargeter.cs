using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerQuestTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.quests;
        }

        public override bool CanGoToTarget => true;
        public override void GoToTarget(object target)
        {
            if (target is NPCQuest quest)
            {
                MainWindow.Instance.mainTabControl.SelectedValue = MainWindow.Instance.questTab;

                var tabber = MainWindow.Instance.questTabSelect;

                for (int i = 0; i < tabber.Items.Count; i++)
                {
                    var tab = tabber.Items[i];

                    if (tab is TabItem tabItem && tabItem.DataContext == quest)
                    {
                        tabber.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCQuest);

            yield return new ReplaceableProperty(nameof(NPCQuest.ID), type, FindReplaceFormats.QUEST_ID);
            yield return new ReplaceableProperty(nameof(NPCQuest.GUID), type, FindReplaceFormats.QUEST_GUID);
        }
    }
}
