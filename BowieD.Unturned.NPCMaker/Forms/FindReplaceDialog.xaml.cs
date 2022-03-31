using BowieD.Unturned.NPCMaker.FindReplace;
using BowieD.Unturned.NPCMaker.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.Forms
{
    /// <summary>
    /// Логика взаимодействия для FindReplaceDialog.xaml
    /// </summary>
    public partial class FindReplaceDialog : Window
    {
        private readonly List<FindReplaceTarget> searchResult = new List<FindReplaceTarget>();
        private bool allowReplace = false;

        public FindReplaceDialog(FindReplaceFormat? initialFormat = null)
        {
            InitializeComponent();

            MainWindow.Instance.MainWindowViewModel.SaveAll();

            FindCommand = new AdvancedCommand(() =>
            {
                searchResult.Clear();

                var format = (FindReplaceFormat)formatsBox.SelectedValue;
                if (FindReplacer.TryParse(format.ValueType, findValueBox.Text, out object findValue))
                {
                    allowReplace = true;

                    var allTargets = FindReplacer.GetAllTargets(format);

                    var matchingTargets = FindReplacer.FindMatchingTargets(allTargets, findValue);

                    searchResult.AddRange(matchingTargets);
                }
                else if (findValueBox.Text.Length == 0)
                {
                    allowReplace = false;

                    var allTargets = FindReplacer.GetAllTargets(format);

                    searchResult.AddRange(allTargets);
                }

                searchResultBox.Items.Clear();

                foreach (var mt in searchResult)
                {
                    searchResultBox.Items.Add(mt);
                }
            }, (p) =>
            {
                if (formatsBox.SelectedIndex < 0)
                    return false;

                return true;
            });

            ReplaceCommand = new AdvancedCommand(() =>
            {
                var format = (FindReplaceFormat)formatsBox.SelectedValue;

                if (FindReplacer.TryParse(format.ValueType, replaceWithValueBox.Text, out var replaceWithValue))
                {
                    FindReplacer.ReplaceInAllTargets(searchResult, replaceWithValue);

                    MainWindow.Instance.MainWindowViewModel.UpdateAllTabs();

                    searchResultBox.Items.Clear();

                    foreach (var item in searchResult)
                    {
                        searchResultBox.Items.Add(item);
                    }
                }
            }, (p) =>
            {
                if (!allowReplace)
                    return false;

                if (formatsBox.SelectedIndex < 0)
                    return false;

                if (searchResult.Count <= 0)
                    return false;

                return true;
            });

            GoToTargetCommand = new AdvancedCommand(() =>
            {
                FindReplaceTarget target = (FindReplaceTarget)searchResultBox.SelectedValue;

                var targeter = target.Targeter;

                if (targeter.CanGoToTarget)
                {
                    if (targeter.ClosesWhenGoesToTarget)
                    {
                        Close();
                    }

                    targeter.GoToTarget(target.Target);
                }
            }, (p) =>
            {
                if (searchResultBox.SelectedIndex >= 0 && searchResultBox.SelectedValue is FindReplaceTarget selectedTarget)
                {
                    var targeter = selectedTarget.Targeter;

                    if (targeter.CanGoToTarget)
                    {
                        return true;
                    }
                }

                return false;
            });

            CloseCommand = new BaseCommand(() =>
            {
                Close();
            });

            DataContext = this;

            formatsBox.Items.Clear();

            foreach (var format in FindReplacer.GetAllFormats())
            {
                formatsBox.Items.Add(format);
            }

            if (initialFormat.HasValue)
            {
                formatsBox.SelectedValue = initialFormat.Value;
            }
        }

        public ICommand FindCommand { get; }
        public ICommand ReplaceCommand { get; }
        public ICommand GoToTargetCommand { get; }
        public ICommand CloseCommand { get; }
    }
}
