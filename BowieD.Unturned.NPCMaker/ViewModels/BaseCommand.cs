using System;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public class BaseCommand : ICommand
    {
        public BaseCommand(Action action)
        {
            this.action = action;
        }
        public BaseCommand(Action<object> action)
        {
            this.actionWithParameter = action;
        }
        private readonly Action action;
        private readonly Action<object> actionWithParameter;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            if (actionWithParameter != null)
                actionWithParameter.Invoke(parameter);
            else
                action?.Invoke();
        }
    }
    public class AdvancedCommand : ICommand
    {
        private Func<object, bool> _canExecute;
        private Action<object> _execute;

        public AdvancedCommand(Action action)
        {
            _execute = new Action<object>((obj) =>
            {
                action.Invoke();
            });
            _canExecute = new Func<object, bool>((obj) => true);
        }
        public AdvancedCommand(Action<object> action)
        {
            _execute = action;
            _canExecute = new Func<object, bool>((obj) => true);
        }
        public AdvancedCommand(Action action, Func<object, bool> canExecute)
        {
            _execute = new Action<object>((obj) =>
            {
                action.Invoke();
            });
            _canExecute = canExecute;
        }
        public AdvancedCommand(Action<object> action, Func<object, bool> canExecute)
        {
            _execute = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
