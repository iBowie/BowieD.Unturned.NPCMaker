using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerDialogueTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.dialogues;
        }

        public override bool CanGoToTarget => true;
        public override void GoToTarget(object target)
        {
            if (target is NPCDialogue dialogue)
            {
                MainWindow.Instance.mainTabControl.SelectedValue = MainWindow.Instance.dialogueTab;
                
                var tabber = MainWindow.Instance.dialogueTabSelect;

                for (int i = 0; i < tabber.Items.Count; i++)
                {
                    var tab = tabber.Items[i];

                    if (tab is TabItem tabItem && tabItem.DataContext == dialogue)
                    {
                        tabber.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCDialogue);

            yield return new ReplaceableProperty(nameof(NPCDialogue.ID), type, FindReplaceFormats.DIALOGUE_ID);
            yield return new ReplaceableProperty(nameof(NPCDialogue.GUID), type, FindReplaceFormats.DIALOGUE_GUID);
        }
    }
}
