using BowieD.Unturned.NPCMaker.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public sealed class LogTabViewModel : BaseViewModel
    {
        public LogTabViewModel()
        {
            _tabVisibility = Visibility.Collapsed;

            var delegateLogger = App.Logger.GetLogger<DelegateLogger>();

            delegateLogger.OnLineAdded += (newLine) =>
            {
                var lst = MainWindow.Instance.logListBox;

                lst.Dispatcher.Invoke(() =>
                {
                    LogLines.Add(newLine);

                    while (LogLines.Count >= 256)
                    {
                        LogLines.RemoveAt(0);
                    }


                    lst.Items.MoveCurrentToLast();
                    lst.ScrollIntoView(lst.Items.CurrentItem);
                });
            };
        }

        private string _userInput;
        public string UserInput
        {
            get => _userInput;
            set => Set(ref _userInput, value, nameof(UserInput));
        }
        private Visibility _tabVisibility;
        public Visibility TabVisibility
        {
            get => _tabVisibility;
            set => Set(ref _tabVisibility, value, nameof(TabVisibility));
        }
        public ObservableCollection<string> LogLines { get; } = new ObservableCollection<string>();

        private ICommand _enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                if (_enterCommand is null)
                {
                    _enterCommand = new AdvancedCommand(() =>
                    {
                        LogLines.Add(Commands.Command.Execute(UserInput));

                        UserInput = string.Empty;
                    }, (p) =>
                    {
                        if (string.IsNullOrWhiteSpace(UserInput))
                            return false;

                        return true;
                    });
                }

                return _enterCommand;
            }
        }
    }
}
