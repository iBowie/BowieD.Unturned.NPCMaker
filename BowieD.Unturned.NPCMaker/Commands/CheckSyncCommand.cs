#if DEBUG
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows.Controls;

namespace BowieD.Unturned.NPCMaker.Commands
{
    public sealed class CheckSyncCommand : Command
    {
        public override string Name => "checksync";
        public override string Help => "Checks sync between tabs and model";
        public override string Syntax => "[character/dialogue/vendor/quest]";
        public override void Execute(string[] args)
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                bool checkResult = true;
                string reason;
                switch (args.Length == 0 ? string.Empty : args[0])
                {
                    case "character":
                        checkResult &= CheckCharacter(out reason);
                        break;
                    case "dialogue":
                        checkResult &= CheckDialogue(out reason);
                        break;
                    case "vendor":
                        checkResult &= CheckVendor(out reason);
                        break;
                    case "quest":
                        checkResult &= CheckQuest(out reason);
                        break;
                    default:
                        checkResult &= CheckCharacter(out reason);
                        if (!checkResult)
                            break;
                        checkResult &= CheckDialogue(out reason);
                        if (!checkResult)
                            break;
                        checkResult &= CheckVendor(out reason);
                        if (!checkResult)
                            break;
                        checkResult &= CheckQuest(out reason);
                        break;
                }
                App.Logger.Log($"checkResult: {checkResult}");
                if (!string.IsNullOrEmpty(reason))
                    App.Logger.Log($"reason: {reason}");
            });
        }

        bool Check<T>(TabControl tabControl, IList<T> items, out string reason)
        {
            // item count check
            if (tabControl.Items.Count != items.Count)
            {
                reason = "Count de-sync";
                return false;
            }

            for (int i = 0; i < tabControl.Items.Count; i++)
            {
                var tab = tabControl.Items[i] as MetroTabItem;
                var item = items[i];

                if (!((T)tab.DataContext).Equals(item))
                {
                    reason = "Item de-sync";
                    return false;
                }
            }
            reason = null;
            return true;
        }

        bool CheckCharacter(out string reason)
        {
            return Check(MainWindow.Instance.characterTabSelect, MainWindow.CurrentProject.data.characters, out reason);
        }
        bool CheckDialogue(out string reason)
        {
            return Check(MainWindow.Instance.dialogueTabSelect, MainWindow.CurrentProject.data.dialogues, out reason);
        }
        bool CheckVendor(out string reason)
        {
            return Check(MainWindow.Instance.vendorTabSelect, MainWindow.CurrentProject.data.vendors, out reason);
        }
        bool CheckQuest(out string reason)
        {
            return Check(MainWindow.Instance.questTabSelect, MainWindow.CurrentProject.data.quests, out reason);
        }
    }
}
#endif