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
        public event EventHandler CanExecuteChanged;
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
}
