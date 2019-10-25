using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public class BaseCommand : ICommand
    {
        public BaseCommand(Action action)
        {
            this.action = action;
        }
        private readonly Action action;
        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(object parameter) => true;
        public virtual void Execute(object parameter)
        {
            action?.Invoke();
        }
    }
}
