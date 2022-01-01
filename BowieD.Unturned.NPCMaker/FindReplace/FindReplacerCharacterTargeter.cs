using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerCharacterTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.characters;
        }

        public override bool CanGoToTarget => true;
        public override void GoToTarget(object target)
        {
            if (target is NPCCharacter character)
            {
                MainWindow.Instance.mainTabControl.SelectedValue = MainWindow.Instance.characterTab;
                
                var tabber = MainWindow.Instance.characterTabSelect;

                for (int i = 0; i < tabber.Items.Count; i++)
                {
                    var tab = tabber.Items[i];

                    if (tab is TabItem tabItem && tabItem.DataContext == character)
                    {
                        tabber.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCCharacter);

            yield return new ReplaceableProperty(nameof(NPCCharacter.ID), type, FindReplaceFormats.CHARACTER_ID);
            yield return new ReplaceableProperty(nameof(NPCCharacter.GUID), type, FindReplaceFormats.CHARACTER_GUID);
            yield return new ReplaceableProperty(nameof(NPCCharacter.startDialogueId), type, FindReplaceFormats.DIALOGUE_ID);
        }
    }
}
