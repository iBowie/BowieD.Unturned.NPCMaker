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
        }
    }
}
