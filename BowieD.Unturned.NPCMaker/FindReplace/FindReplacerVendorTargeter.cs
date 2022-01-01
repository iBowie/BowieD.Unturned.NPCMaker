using BowieD.Unturned.NPCMaker.NPC;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.FindReplace
{
    public sealed class FindReplacerVendorTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.vendors;
        }

        public override bool CanGoToTarget => true;
        public override void GoToTarget(object target)
        {
            if (target is NPCVendor vendor)
            {
                MainWindow.Instance.mainTabControl.SelectedValue = MainWindow.Instance.vendorTab;
                
                var tabber = MainWindow.Instance.vendorTabSelect;

                for (int i = 0; i < tabber.Items.Count; i++)
                {
                    var tab = tabber.Items[i];

                    if (tab is TabItem tabItem && tabItem.DataContext == vendor)
                    {
                        tabber.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(NPCVendor);

            yield return new ReplaceableProperty(nameof(NPCVendor.ID), type, FindReplaceFormats.VENDOR_ID);
            yield return new ReplaceableProperty(nameof(NPCVendor.GUID), type, FindReplaceFormats.VENDOR_GUID);

            yield return new ReplaceableProperty(nameof(NPCVendor.currency), type, FindReplaceFormats.CURRENCY_GUID);
        }
    }
    public sealed class FindReplacerDialogueVendorTargeter : IFindReplacerTargeter
    {
        public override IEnumerable<object> GetTargets()
        {
            return MainWindow.CurrentProject.data.dialogueVendors;
        }

        public override bool CanGoToTarget => true;
        public override void GoToTarget(object target)
        {
            if (target is VirtualDialogueVendor vendor)
            {
                MainWindow.Instance.mainTabControl.SelectedValue = MainWindow.Instance.dialogueVendorTab;

                var tabber = MainWindow.Instance.dialogueVendorTabSelect;

                for (int i = 0; i < tabber.Items.Count; i++)
                {
                    var tab = tabber.Items[i];

                    if (tab is TabItem tabItem && tabItem.DataContext == vendor)
                    {
                        tabber.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        protected override IEnumerable<ReplaceableProperty> CreateReplaceableProperties()
        {
            Type type = typeof(VirtualDialogueVendor);

            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.GoodbyeDialogueID), type, FindReplaceFormats.DIALOGUE_ID);
            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.BoughtDialogueID), type, FindReplaceFormats.DIALOGUE_ID);
            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.SoldDialogueID), type, FindReplaceFormats.DIALOGUE_ID);
            
            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.ID), type, FindReplaceFormats.DIALOGUE_ID);
            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.GUID), type, FindReplaceFormats.DIALOGUE_GUID);

            yield return new ReplaceableProperty(nameof(VirtualDialogueVendor.CurrencyGUID), type, FindReplaceFormats.CURRENCY_GUID);
        }
    }
}
