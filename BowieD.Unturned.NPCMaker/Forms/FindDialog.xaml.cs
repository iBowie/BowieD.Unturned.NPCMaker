using BowieD.Unturned.NPCMaker.Configuration;
using BowieD.Unturned.NPCMaker.FindReplace;
using BowieD.Unturned.NPCMaker.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.Forms
{
	/// <summary>
	/// Interaction logic for FindDialog.xaml
	/// </summary>
	public partial class FindDialog : Window
	{
		private readonly List<FindReplaceTarget> searchResult = new List<FindReplaceTarget>();

		public FindDialog()
		{
			InitializeComponent();

			Width *= AppConfig.Instance.scale;
			Height *= AppConfig.Instance.scale;

			MainWindow.Instance.MainWindowViewModel.SaveAll();

			FindCommand = new AdvancedCommand(() =>
			{
				searchResult.Clear();

				var query = findValueBox.Text;

				var result = FindWithPartialText(query);

				searchResult.AddRange(result);

				searchResultBox.Items.Clear();

				foreach (var mt in searchResult)
				{
					searchResultBox.Items.Add(mt);
				}
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
		}

		public ICommand FindCommand { get; }
		public ICommand GoToTargetCommand { get; }
		public ICommand CloseCommand { get; }

		private static bool CheckString(string query, string text)
		{
			text = text.ToLowerInvariant();

			return text.Contains(query);
		}
		private static IEnumerable<FindReplaceTarget> FindWithPartialText(string query)
		{
			var project = MainWindow.CurrentProject;
			query = query.ToLowerInvariant();

			FindReplacerDialogueTargeter dialogueTargeter = new FindReplacerDialogueTargeter();

			foreach (var dialogue in project.data.dialogues)
			{
				if (dialogue.Responses.Any(d => CheckString(query, d.mainText)))
				{
					yield return new FindReplaceTarget(dialogue, default, dialogueTargeter);
					continue;
				}

				if (dialogue.Messages.Any(d => d.pages.Any(v => CheckString(query, v))))
				{
					yield return new FindReplaceTarget(dialogue, default, dialogueTargeter);
					continue;
				}
			}

			FindReplacerQuestTargeter questTargeter = new FindReplacerQuestTargeter();

			foreach (var quest in project.data.quests)
			{
				if (CheckString(query, quest.Title))
				{
					yield return new FindReplaceTarget(quest, default, questTargeter);
					continue;
				}

				if (CheckString(query, quest.description))
				{
					yield return new FindReplaceTarget(quest, default, questTargeter);
					continue;
				}
			}
		}
	}
}
