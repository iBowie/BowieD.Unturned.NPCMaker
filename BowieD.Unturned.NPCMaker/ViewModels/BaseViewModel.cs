using System.ComponentModel;
using System.Windows.Input;

namespace BowieD.Unturned.NPCMaker.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void RelayChange(string propertyName)
        {
            OnPropertyChange(propertyName);
        }
    }
    public interface INPCTab
    {
        void Save();
        void Reset();
    }
    public interface ITabEditor
    {
        void UpdateTabs();
        ICommand CloseTabCommand { get; }
    }
}
